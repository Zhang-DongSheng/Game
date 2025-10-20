namespace UnityEngine.UI
{
    [RequireComponent(typeof(CanvasRenderer), typeof(ParticleSystem))]
	public class UIEffect : MaskableGraphic
	{
		private const string MainTexPropertyName = "_MainTex";

		private const string BaseMapPropertyName = "_BaseMap";

		[SerializeField] private UiParticleRenderMode _mode = UiParticleRenderMode.Billboard;

		[SerializeField] private float speed = 1f;

		[SerializeField] private float length = 1f;

		[SerializeField] private bool ignoreTimeScale = false;

		[SerializeField] private Mesh _mesh;                                                                

		private ParticleSystem _particle;

		private ParticleSystemRenderer _renderer;

		private int _mainTexture = -1;

		private ParticleSystem.Particle[] particles;

		private Vector3[] verts;

		private int[] triangles;

		private Vector2[] uvs;

        private Mesh mesh;

        private readonly Vector2[] uv4 = new Vector2[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };

		public ParticleSystem Particle
		{
			get { return _particle; }
			set
			{
				if (value != _particle)
				{
					SetAllDirty();
				}
				_particle = value;
			}
		}

		public ParticleSystemRenderer Renderer
		{
			get
			{
				return _renderer;
			}
			set
			{
				_renderer = value;
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if (material != null)
				{
					if (_mainTexture == -1)
					{
						if (material.HasProperty(MainTexPropertyName))
							_mainTexture = Shader.PropertyToID(MainTexPropertyName);
						else if (material.HasProperty(BaseMapPropertyName))
							_mainTexture = Shader.PropertyToID(BaseMapPropertyName);
					}

					return material.GetTexture(_mainTexture);
				}

				return s_WhiteTexture;
			}
		}

		public UiParticleRenderMode RenderMode
		{
			get { return _mode; }
			set
			{
				_mode = value; SetAllDirty();
			}
		}

		public Mesh Mesh
		{
			get { return _mesh; }
			set
			{
				if (value != _mesh)
				{
					InitMeshData(); SetAllDirty();
				}
				_mesh = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			Particle = GetComponent<ParticleSystem>();

			Renderer = GetComponent<ParticleSystemRenderer>();

			if (Renderer == null) return;

			if (m_Material == null)
				m_Material = Renderer.sharedMaterial;
			if (Renderer.renderMode == ParticleSystemRenderMode.Stretch)
				RenderMode = UiParticleRenderMode.StreachedBillboard;
			if (Renderer.enabled)
				Renderer.enabled = false;

			InitMeshData();
		}

		protected void Update()
		{
			if (Particle == null) return;

			if (ignoreTimeScale)
			{
				Particle.Simulate(Time.unscaledDeltaTime, true, false);
			}
			else if (!Particle.isPlaying)
			{
				return;
			}
			SetVerticesDirty();
		}

		protected override void OnPopulateMesh(VertexHelper helper)
		{
			if (Particle == null)
			{
				base.OnPopulateMesh(helper);
			}
			else
			{
				GenerateParticlesBillboards(helper);
			}
		}

		public override void SetMaterialDirty()
		{
			base.SetMaterialDirty();
			if (_renderer != null)
				_renderer.sharedMaterial = m_Material;
		}

		private void InitMeshData()
		{
			if (Mesh != null && Mesh != mesh)
			{
				mesh = Mesh;
				verts = mesh.vertices;
				uvs = mesh.uv;
				triangles = mesh.triangles;
			}
		}

		private void GenerateParticlesBillboards(VertexHelper vh)
		{
			var textureSheetAnimationModule = Particle.textureSheetAnimation;

			if (particles == null || particles.Length < Particle.main.maxParticles)
			{
				particles = new ParticleSystem.Particle[Particle.main.maxParticles];
			}

			int numParticlesAlive = Particle.GetParticles(particles);

			vh.Clear();

			//!NOTE sample curves before render particles, because it produces allocations
			var frameOverTime = Particle.textureSheetAnimation.frameOverTime;
			var velocityOverLifeTime = Particle.velocityOverLifetime;
			var velocityOverTimeX = velocityOverLifeTime.x;
			var velocityOverTimeY = velocityOverLifeTime.y;
			var velocityOverTimeZ = velocityOverLifeTime.z;
			var isWorldSimulationSpace = Particle.main.simulationSpace == ParticleSystemSimulationSpace.World;

			if (RenderMode == UiParticleRenderMode.Mesh)
			{
				if (Mesh != null)
				{
					InitMeshData();
					for (int i = 0; i < numParticlesAlive; i++)
					{
						DrawParticleMesh(particles[i], vh, frameOverTime, isWorldSimulationSpace,
							textureSheetAnimationModule, verts, triangles, uvs);
					}
				}
			}
			else
			{
				for (int i = 0; i < numParticlesAlive; i++)
				{
					DrawParticleBillboard(particles[i], vh, frameOverTime,
						velocityOverTimeX, velocityOverTimeY, velocityOverTimeZ, isWorldSimulationSpace,
						textureSheetAnimationModule);
				}
			}
		}

		private void DrawParticleMesh(ParticleSystem.Particle particle, VertexHelper vh,
			ParticleSystem.MinMaxCurve frameOverTime, bool isWorldSimulationSpace,
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule,
			Vector3[] verts, int[] triangles, Vector2[] uvs)
		{
			var center = particle.position;
			var rotation = Quaternion.Euler(particle.rotation3D);

			if (isWorldSimulationSpace)
			{
				center = rectTransform.InverseTransformPoint(center);
			}

			float timeAlive = particle.startLifetime - particle.remainingLifetime;

			Vector3 size3D = particle.GetCurrentSize3D(Particle);

			Color32 color32 = particle.GetCurrentColor(Particle);

			CalculateUvs(particle, frameOverTime, textureSheetAnimationModule, timeAlive, out uv4[0], out uv4[1], out uv4[2], out uv4[3]);

			int count = vh.currentVertCount;

			Vector3 position;

			Vector2 point = new Vector2();

			for (int j = 0; j < verts.Length; j++)
			{
				position = verts[j];
				position.x *= size3D.x;
				position.y *= size3D.y;
				position.z *= size3D.z;
				position = rotation * position + center;

				point.x = Mathf.Lerp(uv4[0].x, uv4[2].x, uvs[j].x);
				point.y = Mathf.Lerp(uv4[0].y, uv4[2].y, uvs[j].y);

				vh.AddVert(position, color32, point);
			}

			for (int i = 0; i < triangles.Length; i += 3)
			{
				vh.AddTriangle(count + triangles[i], count + triangles[i + 1], count + triangles[i + 2]);
			}
		}

		private void DrawParticleBillboard(ParticleSystem.Particle particle, VertexHelper vh,
			ParticleSystem.MinMaxCurve frameOverTime,
			ParticleSystem.MinMaxCurve velocityOverTimeX,
			ParticleSystem.MinMaxCurve velocityOverTimeY,
			ParticleSystem.MinMaxCurve velocityOverTimeZ,
			bool isWorldSimulationSpace,
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule)
		{
			var center = particle.position;
			var rotation = Quaternion.Euler(particle.rotation3D);

			if (isWorldSimulationSpace)
			{
				center = rectTransform.InverseTransformPoint(center);
			}

			float timeAlive = particle.startLifetime - particle.remainingLifetime;
			float globalTimeAlive = timeAlive / particle.startLifetime;

			Vector3 size3D = particle.GetCurrentSize3D(Particle);

			if (_mode == UiParticleRenderMode.StreachedBillboard)
			{
				GetStrechedBillboardsSizeAndRotation(particle, globalTimeAlive, ref size3D, out rotation,
					velocityOverTimeX, velocityOverTimeY, velocityOverTimeZ);
			}

			var leftTop = new Vector3(-size3D.x * 0.5f, size3D.y * 0.5f);
			var rightTop = new Vector3(size3D.x * 0.5f, size3D.y * 0.5f);
			var rightBottom = new Vector3(size3D.x * 0.5f, -size3D.y * 0.5f);
			var leftBottom = new Vector3(-size3D.x * 0.5f, -size3D.y * 0.5f);

			leftTop = rotation * leftTop + center;
			rightTop = rotation * rightTop + center;
			rightBottom = rotation * rightBottom + center;
			leftBottom = rotation * leftBottom + center;

			Color32 color32 = particle.GetCurrentColor(Particle);

			int count = vh.currentVertCount;

			CalculateUvs(particle, frameOverTime, textureSheetAnimationModule, timeAlive, out uv4[0], out uv4[1], out uv4[2], out uv4[3]);

			vh.AddVert(leftBottom, color32, uv4[0]);
			vh.AddVert(leftTop, color32, uv4[1]);
			vh.AddVert(rightTop, color32, uv4[2]);
			vh.AddVert(rightBottom, color32, uv4[3]);

			vh.AddTriangle(count, count + 1, count + 2);
			vh.AddTriangle(count + 2, count + 3, count);
		}

		private static void CalculateUvs(ParticleSystem.Particle particle, ParticleSystem.MinMaxCurve frameOverTime,
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule,
			float timeAlive, out Vector2 uv0, out Vector2 uv1, out Vector2 uv2, out Vector2 uv3)
		{
			if (!textureSheetAnimationModule.enabled)
			{
				uv0 = new Vector2(0f, 0f);
				uv1 = new Vector2(0f, 1f);
				uv2 = new Vector2(1f, 1f);
				uv3 = new Vector2(1f, 0f);
			}
			else
			{
				float lifeTimePerCycle = particle.startLifetime / textureSheetAnimationModule.cycleCount;
				float timePerCycle = timeAlive % lifeTimePerCycle;
				float timeAliveAnim01 = timePerCycle / lifeTimePerCycle; // in percents

				var totalFramesCount = textureSheetAnimationModule.numTilesY * textureSheetAnimationModule.numTilesX;
				var frame01 = frameOverTime.Evaluate(timeAliveAnim01);

				var frame = 0f;
				switch (textureSheetAnimationModule.animation)
				{
					case ParticleSystemAnimationType.WholeSheet:
						{
							frame = Mathf.Clamp(Mathf.Floor(frame01 * totalFramesCount), 0, totalFramesCount - 1);
							break;
						}
					case ParticleSystemAnimationType.SingleRow:
						{
							frame = Mathf.Clamp(Mathf.Floor(frame01 * textureSheetAnimationModule.numTilesX), 0,
								textureSheetAnimationModule.numTilesX - 1);
							int row = textureSheetAnimationModule.rowIndex;
							if (textureSheetAnimationModule.rowMode == ParticleSystemAnimationRowMode.Random)
							{
								Random.InitState((int)particle.randomSeed);
								row = Random.Range(0, textureSheetAnimationModule.numTilesY);
							}
							frame += row * textureSheetAnimationModule.numTilesX;
							break;
						}
				}

				int x = (int)frame % textureSheetAnimationModule.numTilesX;
				int y = (int)frame / textureSheetAnimationModule.numTilesX;

				float xDelta = 1f / textureSheetAnimationModule.numTilesX;
				float yDelta = 1f / textureSheetAnimationModule.numTilesY;

				y = textureSheetAnimationModule.numTilesY - 1 - y;

				float sX = x * xDelta;
				float sY = y * yDelta;
				float eX = sX + xDelta;
				float eY = sY + yDelta;

				uv0 = new Vector2(sX, sY);
				uv1 = new Vector2(sX, eY);
				uv2 = new Vector2(eX, eY);
				uv3 = new Vector2(eX, sY);
			}
		}

		private void GetStrechedBillboardsSizeAndRotation(ParticleSystem.Particle particle, float timeAlive01,
			ref Vector3 size3D, out Quaternion rotation,
			ParticleSystem.MinMaxCurve x, ParticleSystem.MinMaxCurve y, ParticleSystem.MinMaxCurve z)
		{
			var velocityOverLifeTime = Vector3.zero;

			if (Particle.velocityOverLifetime.enabled)
			{
				velocityOverLifeTime.x = x.Evaluate(timeAlive01);
				velocityOverLifeTime.y = y.Evaluate(timeAlive01);
				velocityOverLifeTime.z = z.Evaluate(timeAlive01);
			}

			var finalVelocity = particle.velocity + velocityOverLifeTime;
			var ang = Vector3.Angle(finalVelocity, Vector3.up);
			var horizontalDirection = finalVelocity.x < 0 ? 1 : -1;
			rotation = Quaternion.Euler(new Vector3(0, 0, ang * horizontalDirection));
			size3D.y *= length;
			size3D += new Vector3(0, speed * finalVelocity.magnitude);
		}
	}

	public enum UiParticleRenderMode
	{
		Billboard,
		StreachedBillboard,
		Mesh
	}
}