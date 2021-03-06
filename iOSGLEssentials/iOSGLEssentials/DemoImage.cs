using System;
using OpenTK.Graphics.ES20;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace iOSGLEssentials
{
	public class DemoImage
	{
		public byte[] Data {get; set;}
		public int Size {get; set;}
		
		public int Width {get; set;}
		public int Height {get; set;}
		public PixelInternalFormat Format {get; set;}
		public PixelType Type {get; set;}
		
		public int RowByteSize{get; set;}
			
		public DemoImage ()
		{
		}
		
		public unsafe static DemoImage LoadImage(string filePathName, bool flipVertical)
		{
			var imageClass = UIImage.FromFile(filePathName);
			
			var cgImage = imageClass.CGImage;

			if(cgImage == null)
			{
				return null;
			}
			
			var image = new DemoImage();
			image.Width = cgImage.Width;
			image.Height = cgImage.Height;
			image.RowByteSize = image.Width * 4;
			image.Data =  new byte[cgImage.Height * image.RowByteSize]; 
			image.Format = PixelInternalFormat.Rgba;
			image.Type = PixelType.UnsignedByte;
			
			fixed (byte *ptr = &image.Data [0]){
				using(var context = new CGBitmapContext((IntPtr) ptr, image.Width, image.Height, 8, image.RowByteSize, cgImage.ColorSpace, CGImageAlphaInfo.NoneSkipLast))
				{
					context.SetBlendMode(CGBlendMode.Copy);
					
					if(flipVertical)
					{
						context.TranslateCTM(0.0f, (float)image.Height);
						context.ScaleCTM(1.0f, -1.0f);
					}
					
					context.DrawImage(new RectangleF(0f, 0f, image.Width, image.Height), cgImage);
				}
			}
			return image;
		}
	}
}

