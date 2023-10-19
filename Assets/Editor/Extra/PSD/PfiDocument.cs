using SubjectNerd.PsdImporter.PsdParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pfi
{
	public class PfiDocument
	{
		public string name;

		public Vector2 size;

		public PfiFolder root;

		public override string ToString()
		{
			return this.name;
		}
	}
}