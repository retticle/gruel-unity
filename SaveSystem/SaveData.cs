using System.Collections.Generic;

namespace Gruel.SaveSystem {
	public class SaveData {

		public void Init() {
			_musicEnabled = true;
			_sfxEnabled = true;
		
			_chips = 0;
			_equippedCloak = "com.supermegaquest.ancestor.novice";
			_equippedOrb = "com.supermegaquest.ancestor.lux";
			_equippedProjectile = "com.supermegaquest.ancestor.projectile.sonic";
		
			_cloaks = new List<string> {
				"com.supermegaquest.ancestor.novice",
			};

			_orbs = new List<string> {
				"com.supermegaquest.ancestor.lux",
			};

			_projectiles = new List<string> {
				"com.supermegaquest.ancestor.projectile.sonic",
			};
		}

		public bool _musicEnabled;
		public bool _sfxEnabled;

		public int _chips;
		public string _equippedCloak;
		public string _equippedOrb;
		public string _equippedProjectile;
	
		public List<string> _cloaks;
		public List<string> _orbs;
		public List<string> _projectiles;
	
	}
}