using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Console = HUDConsole.Console;

public class SaveController {
	
#region Init
	public SaveController() {
		_savePath = Application.persistentDataPath + "/save.json";
		Load();
		
		Console.AddCommand("AddChips", ConsoleAddChips, "Adds chips");
	}
#endregion Init
	
#region Load/Save
	private static string _savePath = string.Empty;
	private static SaveData _saveData { get; set; }

	private static void Load() {
		Debug.Log("SaveController.Load");
		
		if (File.Exists(_savePath)) {
			var saveString = string.Empty;
			
			// Load save file JSON.
			using (var fs = new FileStream(_savePath, FileMode.Open)) {
				using (var reader = new StreamReader(fs)) {
					saveString = reader.ReadToEnd();
				}
			}

			// Deserialize JSON to SaveData class.
			_saveData = JsonConvert.DeserializeObject<SaveData>(saveString);
		} else {
			CreateNewSave();
		}
	}

	public static void Save() {
		Debug.Log("SaveController.Save");
		
		// Serialize _saveData to JSON.
		var saveString = JsonConvert.SerializeObject(_saveData, Formatting.Indented);

		using (var fs = new FileStream(_savePath, FileMode.Create)) {
			using (var writer = new StreamWriter(fs)) {
				writer.Write(saveString);
			}
		}
	}

	private static void CreateNewSave() {
		if (_saveData == null) {
			_saveData = new SaveData();
			_saveData.Init();
		}
		
		Save();
	}
#endregion Load/Save
	
#region Settings
	public static Action<bool> _onMusicEnabledChanged;
	public static Action<bool> _onSFXEnabledChanged;
	
	public static bool MusicEnabled {
		get { return _saveData._musicEnabled; }
		set {
			_saveData._musicEnabled = value;
			
			Save();
			_onMusicEnabledChanged?.Invoke(_saveData._musicEnabled);
		}
	}

	public static bool SFXEnabled {
		get { return _saveData._sfxEnabled; }
		set {
			_saveData._sfxEnabled = value;
			
			Save();
			_onSFXEnabledChanged?.Invoke(_saveData._sfxEnabled);
		}
	}
#endregion Settings
	
#region Chips
	public static Action<int, int> _onChipsChanged;

	public static int GetChips() {
		return _saveData._chips;
	}
	
	public static void AdjustChips(int delta) {
		_saveData._chips += delta;
		
		Debug.Log($"SaveController.AdjustChips: adjusted-value: {_saveData._chips} delta: {delta}");

		Save();
		_onChipsChanged?.Invoke(_saveData._chips, delta);
	}

	private void ConsoleAddChips(string[] args) {
		AdjustChips(int.Parse(args[0]));
	}
#endregion Chips
	
#region Items
	public static Action _onItemUnlocked;
#endregion Items
	
#region Cloak
	public static Action<string> _onCloakUnlocked;
	public static Action<string> _onCloakEquipped;

	public static string GetEquippedCloak() {
		return _saveData._equippedCloak;
	}

	public static void SetEquippedCloak(string itemId) {
		Debug.Log($"SaveController.SetEquippedCloak: itemId: {itemId}");
		
		_saveData._equippedCloak = itemId;
		
		Save();
		_onCloakEquipped?.Invoke(itemId);
	}
	
	public static bool GetCloakUnlocked(string itemId) {
		return _saveData._cloaks.Contains(itemId);
	}
	
	public static void SetCloakUnlocked(string itemId) {
		Debug.Log($"SaveController.SetCloakUnlocked: itemId: {itemId}");
		
		if (_saveData._cloaks.Contains(itemId) == false) {
			_saveData._cloaks.Add(itemId);
		}
		
		Save();
		_onCloakUnlocked?.Invoke(itemId);
		_onItemUnlocked?.Invoke();
	}
#endregion Cloak
	
#region Orbs
	public static Action<string> _onOrbUnlocked;
	public static Action<string> _onOrbEquipped;

	public static string GetEquippedOrb() {
		return _saveData._equippedOrb;
	}

	public static void SetEquippedOrb(string itemId) {
		Debug.Log($"SaveController.SetEquippedOrb: itemId: {itemId}");

		_saveData._equippedOrb = itemId;
		
		Save();
		_onOrbEquipped?.Invoke(itemId);
	}

	public static bool GetOrbUnlocked(string itemId) {
		return _saveData._orbs.Contains(itemId);
	}

	public static void SetOrbUnlocked(string itemId) {
		Debug.Log($"SaveController.SetOrbUnlocked: itemId: {itemId}");

		if (_saveData._orbs.Contains(itemId) == false) {
			_saveData._orbs.Add(itemId);
		}
		
		Save();
		_onOrbUnlocked?.Invoke(itemId);
		_onItemUnlocked?.Invoke();
	}
#endregion Orbs
	
#region Projectiles
	public static Action<string> _onProjectileUnlocked;
	public static Action<string> _onProjectileEquipped;

	public static string GetEquippedProjectile() {
		return _saveData._equippedProjectile;
	}

	public static void SetEquippedProjectile(string itemId) {
		Debug.Log($"SaveController.SetEquippedProjectile: itemId: {itemId}");

		_saveData._equippedProjectile = itemId;
		
		Save();
		_onProjectileEquipped?.Invoke(itemId);
	}

	public static bool GetProjectileUnlocked(string itemId) {
		return _saveData._projectiles.Contains(itemId);
	}

	public static void SetProjectileUnlocked(string itemId) {
		Debug.Log($"SaveController.SetProjectileUnlocked: itemId: {itemId}");

		if (_saveData._projectiles.Contains(itemId) == false) {
			_saveData._projectiles.Add(itemId);
		}
		
		Save();
		_onProjectileUnlocked?.Invoke(itemId);
		_onItemUnlocked?.Invoke();
	}
#endregion Projectiles

}