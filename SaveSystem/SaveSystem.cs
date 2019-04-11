using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Gruel.SaveSystem {
	public class SaveSystem {
	
#region Init
		public SaveSystem() {
			_savePath = Application.persistentDataPath + "/save.json";
			Load();
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

	}
}