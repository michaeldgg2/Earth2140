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
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.E2140.Traits;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
[Desc("Makes an actor play a sound while moving.")]
public class WithMoveSoundInfo : TraitInfo
{
	[FieldLoader.RequireAttribute]
	public readonly string Sound = "";

	public override object Create(ActorInitializer init)
	{
		return new WithMoveSound(this);
	}
}

public class WithMoveSound : INotifyMoving, ITick
{
	private readonly WithMoveSoundInfo info;
	private ISound? sound;

	public WithMoveSound(WithMoveSoundInfo info)
	{
		this.info = info;
	}

	void INotifyMoving.MovementTypeChanged(Actor self, MovementType type)
	{
		if (type.HasFlag(MovementType.Horizontal))
			this.sound ??= Game.Sound.PlayLooped(SoundType.World, this.info.Sound, self.CenterPosition);
		else
		{
			Game.Sound.StopSound(this.sound);
			this.sound = null;
		}
	}

	void ITick.Tick(Actor self)
	{
		this.sound?.SetPosition(self.CenterPosition);
	}
}
