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

using JetBrains.Annotations;
using OpenRA.Graphics;
using OpenRA.Traits;

namespace OpenRA.Mods.E2140.Traits.Palettes;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
[TraitLocation(SystemActors.World | SystemActors.EditorWorld)]
[Desc("Creates the OpenE2140 special effects palette.")]
public class OpenE2140PaletteInfo : TraitInfo
{
	[PaletteDefinition]
	[FieldLoader.RequireAttribute]
	[Desc("Internal palette name")]
	public readonly string? Name;

	public readonly bool AllowModifiers = true;

	public override object Create(ActorInitializer init) { return new OpenE2140Palette(this); }
}

public class OpenE2140Palette : ILoadsPalettes
{
	private readonly OpenE2140PaletteInfo info;

	public OpenE2140Palette(OpenE2140PaletteInfo info)
	{
		this.info = info;
	}

	void ILoadsPalettes.LoadPalettes(WorldRenderer worldRenderer)
	{
		var colors = new uint[Palette.Size];

		// Tracks
		colors[240] = 0xff181c18;
		colors[241] = 0xff212421;
		colors[242] = 0xff181c18;
		colors[243] = 0xff292c29;

		// Effects
		colors[244] = 0xffff9e52;
		colors[245] = 0xffefb68c;
		colors[246] = 0xffffebc6;
		colors[247] = 0xffffffff;

		// Player colors
		for (var i = 248; i <= 252; i++)
		{
			var gray = 0x80 + (i - 250) * 0x18;

			colors[i] = (uint)((0xff << 24) | (gray << 16) | (gray << 8) | gray);
		}

		worldRenderer.AddPalette(this.info.Name, new ImmutablePalette(colors), this.info.AllowModifiers);
	}
}
