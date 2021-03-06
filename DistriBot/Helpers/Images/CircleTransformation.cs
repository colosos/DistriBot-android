﻿using System;
using Android.Graphics;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace DistriBot
{
	public class CircleTransformation : TransformationBase
	{

		public double BorderSize { get; set; }
		public string BorderHexColor { get; set; }

		public CircleTransformation() : this(0d, null)
		{
		}

		public CircleTransformation(double borderSize, string borderHexColor)
		{
			BorderSize = borderSize;
			BorderHexColor = borderHexColor;
		}

		public override string Key
		{
			get { return string.Format("CircleTransformation,borderSize={0},borderHexColor={1}", BorderSize, BorderHexColor); }
		}

		protected override Bitmap Transform(Bitmap source)
		{
			return RoundedTransformation.ToRounded(source, 0f, 1f, 1f, BorderSize, BorderHexColor);
		}
	}
}

