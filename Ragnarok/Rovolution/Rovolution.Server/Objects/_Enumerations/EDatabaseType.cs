﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rovolution.Server {

	[Flags()]
	public enum EDatabaseType {
		None = 0,
		All = 0,

		Account = 1,
		Char = 2,
		Skill = 4,
		Party = 8,
		Guild = 16,

		Mob = 32,
		Item = 64,
		Npc = 128,

		Homunculus = 256,
		Pet = 512,
		Mercenary = 1024,

		// Only used for world objects
		Chat = 2048,
		Battleground = 4096,

		// All types with regerneration
		Regeneration = (EDatabaseType.Char | EDatabaseType.Homunculus | EDatabaseType.Mercenary),
		// Define to determine who gets HP/SP consumed on doing skills/etc
		Consume = (EDatabaseType.Char | EDatabaseType.Homunculus | EDatabaseType.Mercenary),
	}


	public static class EDatabaseTypeExtension {

		/// <summary>
		/// Simplifies the bit-type checking, should be implemented for all bit-flag enums..
		/// </summary>
		/// <param name="type"></param>
		/// <param name="test"></param>
		/// <returns></returns>
		public static bool And(this EDatabaseType type, EDatabaseType test) {
			return (type & test) > 0;
		}

	}

}
