#region Copyright & License Information

/*
 * Copyright 2007-2023 The OpenE2140 Developers (see AUTHORS)
 * This file is part of OpenE2140, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */

#endregion

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using OpenRA.Graphics;
using OpenRA.Mods.E2140.FileFormats;
using OpenRA.Primitives;

namespace OpenRA.Mods.E2140.SpriteLoaders;

public class DatSpriteFrame : ISpriteFrame
{
	public SpriteFrameType Type { get; }
	public Size Size { get; }
	public Size FrameSize { get; }
	public float2 Offset { get; }
	public byte[] Data { get; }
	public bool DisableExportPadding => true;

	public DatSpriteFrame(SpriteFrameType type, Size size, byte[] pixels)
	{
		this.Type = type;
		this.Size = size;
		this.FrameSize = size;
		this.Offset = new float2(0, 0);
		this.Data = pixels;
	}
}

[UsedImplicitly]
public class DatSpriteLoader : ISpriteLoader
{
	public bool TryParseSprite(Stream stream, string filename, [NotNullWhen(true)] out ISpriteFrame[]? frames, out TypeDictionary? metadata)
	{
		frames = null;
		metadata = null;

		if (!filename.EndsWith(".dat", StringComparison.OrdinalIgnoreCase))
			return false;

		var datImage = new DatImage(stream);

		// We have no access to the parent container, so we need to re-parse it here :/
		var baseStream = ((SegmentStream)stream).BaseStream;
		baseStream.Position = 0;
		var wd = new Wd(baseStream, "HACK", null);

		var palPalette = new PalPalette(wd.GetStream(filename[..^4].Split('|').Last() + ".PAL") ?? throw new FileNotFoundException());

		frames = new ISpriteFrame[]
		{
			new DatSpriteFrame(
				SpriteFrameType.Rgba32,
				new Size(datImage.Width, datImage.Height),
				datImage.Pixels.SelectMany(
						i =>
						{
							var color = palPalette.Colors[i];

							return new[] { color.R, color.G, color.B, color.A };
						}
					)
					.ToArray()
			)
		};

		return true;
	}
}
