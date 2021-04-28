using System.Collections.Generic;
using UnityEngine.Sprites;

namespace UnityEngine.UI
{
    public class Circle : Image
    {
        [SerializeField] private int segements = 100;

        [SerializeField] private int showPercent = 1;

        [SerializeField] private bool accurateRaycast;

        private readonly Color32 GRAY = new Color32(60, 60, 60, 255);

        private readonly List<Vector3> vertexs = new List<Vector3>();

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear(); vertexs.Clear();

            AddVertex(helper);

            AddTriangle(helper);
        }

        private void AddVertex(VertexHelper helper)
        {
            //��ȡͼ�εĿ�͸�
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            int realSegements = (int)(segements * showPercent);//��ʵҪ��ʾ�����εĸ���

            Vector4 uv = overrideSprite != null ? DataUtility.GetInnerUV(overrideSprite) : Vector4.zero;//��ȡsprite uv
            
            //����uv��͸�
            float uvWidth = uv.z - uv.x;
            float uvHeight = uv.w - uv.y;
            Vector2 uvCenter = new Vector2((uv.x + uv.z) * 0.5f, (uv.y + uv.w) * 0.5f);//uv���ĵ����� 
            Vector2 convertRatio = new Vector2(uvWidth / width, uvHeight / height);//uv��UI�����ת����

            //����ÿ���Ӧ�Ļ��Ⱥͼ���뾶
            float radian = (2 * Mathf.PI) / segements;//���� һ����Բ������2�� 
            float radius = width * 0.5f;//�뾶
                                        //����ÿ�������ζ������� 1.����Բ�ĵ���Ϣ(���������ι��ö���) 2.��Բ�Ķ����������������������Ϣ ����sin@ cos@ ����

            Vector2 originPos = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);//Բ�Ķ���UI���� Ĭ�����ĵ�(0.5,0.5) Ĭ������(0,0)
            Vector2 originuvPos = Vector2.zero;//Բ�Ķ���uv����
            Color32 colorTemp = GetOriginColor();

            UIVertex origin = GetUIVertex(colorTemp, originPos, originuvPos, uvCenter, convertRatio); //Բ�ĵĶ�����Ϣ
            helper.AddVert(origin);

            int vertexCount = realSegements + 1;//��ʵ�������
            if (showPercent == 0)//��ʾ����0 Ҫ��ʾ�������==0
                vertexCount = 0;

            float curRadian = 0;//��ǰ�Ļ���
            Vector2 posTermp = Vector2.zero;
            for (int i = 0; i < segements + 1; i++)//��ʾ���ж���
            {
                float x = Mathf.Cos(curRadian) * radius;
                float y = Mathf.Sin(curRadian) * radius;
                curRadian += radian;//�����¸�����

                if (i < vertexCount)
                {
                    colorTemp = color;
                }
                else//����ʾ���� ������ɫ���ñ��
                {
                    colorTemp = GRAY;
                }
                posTermp = new Vector2(x, y);//���ö���UI����
                UIVertex vertexTemp = GetUIVertex(colorTemp, posTermp + originPos, posTermp, uvCenter, convertRatio);//�����¸�����
                helper.AddVert(vertexTemp);
                if (accurateRaycast)
                    vertexs.Add(posTermp + originPos);
            }
        }

        private void AddTriangle(VertexHelper helper)
        {
            int index = 0;

            for (int i = 0; i < segements; i++)
            {
                index++;

                if (index == segements)
                {
                    helper.AddTriangle(index, 0, 1);
                }
                else
                {
                    helper.AddTriangle(index, 0, index + 1);
                }
            }
        }

        /// <summary>
        /// �õ�Բ�Ķ������ɫ
        /// </summary>
        /// <returns></returns>
        private Color32 GetOriginColor()
        {
            Color32 colorTemp = (Color.white - GRAY) * showPercent;
            colorTemp = new Color32((byte)(GRAY.r + colorTemp.r), (byte)(GRAY.g + colorTemp.g), (byte)(GRAY.b + colorTemp.b), 255);
            return colorTemp;
        }

        /// <summary>
        /// ��ȡUV����
        /// </summary>
        private UIVertex GetUIVertex(Color32 col, Vector3 pos, Vector2 uvPos, Vector2 uvCenter, Vector2 uvScale)
        {
            UIVertex vertex = new UIVertex();
            vertex.color = col;//���ö�����ɫ
            vertex.position = pos;//���ö���λ��
                                  //���㶥��λ�ö�Ӧ��ͼƬ��uv����--����pos--uv==��ת����ʽ ��uv.x=pos.x*ratio.x+uvCenter.x --��uvCenter.x ��ֹ���ĵ�ı� ͼƬƫ��
            vertex.uv0 = new Vector2(uvPos.x * uvScale.x + uvCenter.x, uvPos.y * uvScale.y + uvCenter.y);//���ö���uv
            return vertex;
        }

        /// <summary>
        /// ��д���߼�ⷽ��
        /// </summary>
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (accurateRaycast)//������׼���߼��
            {
                //�������Ļ�ĵ�-->�ֲ�����
                Vector2 clickPos = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out clickPos);
                //�жϵ���ĵ� �Ƿ���Ч   
                //�㷨ԭ��ͼ�������ڵ�2�����һ��ֱ��  �жϵ���ĵ����ҷ�������� ��ֱ���ཻ�� �ǲ������� �������� ����ͼ���ڲ� ż������ ����ͼ���ⲿ
                bool flag = GetCrossPointNum(clickPos, vertexs) % 2 == 1 ? true : false;
                return flag;
            }
            else//Ĭ�����߼��
            {
                return base.IsRaycastLocationValid(screenPoint, eventCamera);
            }
        }

        /// <summary>
        /// �õ�����ֱ���ཻ��ĸ���
        /// </summary>
        /// <param name="clickPos">�����Ļ�ĵ�ľֲ�����</param>
        /// <param name="_vertexList">��ɵ�ͼ�ε����е�(����Բ�ε�)</param>
        /// <returns></returns>
        private int GetCrossPointNum(Vector2 clickPos, List<Vector3> _vertexList)
        {
            int crossCount = 0; //�ཻ�����
            Vector3 vert1 = Vector3.zero;
            Vector3 vert2 = Vector3.zero;
            int vertexCount = _vertexList.Count;//���ͼ�ζ������
                                                //�������ж��� �����е�������2������ɵ�ֱ��
            for (int i = 0; i < vertexCount; i++)
            {
                vert1 = _vertexList[i];
                vert2 = _vertexList[(i + 1) % vertexCount];//ȡ�� ��ֹԽ�� �� ����β����
                if (IsYInRang(clickPos, vert1, vert2))//����ĵ� ��ֱ��Y�᷶Χ��
                {
                    float x = GetX(vert1, vert2, clickPos.y);
                    if (clickPos.x < x)//�������ҵ�����
                    {
                        crossCount++;
                    }
                }
            }
            return crossCount;
        }

        /// <summary>
        /// ����ĵ��Ƿ���Y����Ч��Χ��
        /// </summary>
        private bool IsYInRang(Vector2 clickPos, Vector3 vert1, Vector3 vert2)
        {
            if (vert1.y > vert2.y)
            {
                return clickPos.y < vert1.y && clickPos.y > vert2.y;
            }
            else
            {
                return clickPos.y > vert1.y && clickPos.y < vert2.y;
            }
        }

        /// <summary>
        /// �õ��ཻ�������X�� Y����֪
        /// </summary>
        private float GetX(Vector3 vert1, Vector3 vert2, float y)
        {
            //����ֱ�߹�ʽ б��ʽ��y=k*x+b    k=(y1-y2)/(x1-x2)-->k��֪ ����֪һ��(x1,y1) ��y--������x
            float k = (vert1.x - vert2.x) / (vert1.y - vert2.y);//����б��k
                                                                //��2�� (x,y) ��vert2.x��vert2.y��
            float x = (k / (y - vert2.y)) + vert2.x;
            return x;
        }
    }
}
