using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Gruel {
	public static class ExtensionMethods {
	
		public static void Shuffle<T>(this IList<T> list, System.Random rnd) {
			for (int i = 0, n = list.Count; i < n; i++) {
				list.Swap(i, rnd.Next(i, list.Count));
			}
		}

		public static void Swap<T>(this IList<T> list, int i, int j) {
			var temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}

		public static float Remap(this float value, float startLow, float startHigh, float endLow, float endHigh) {
			return (value - startLow) / (startHigh - startLow) * (endHigh - endLow) + endLow;
		}

		public static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
			Vector2 _difference = vec2 - vec1;
			float _sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
			return Vector2.Angle(Vector2.right, _difference) * _sign;
		}

		public static string RemoveCapCharacters(string @string) {
			var _charArray = @string.ToCharArray();

			var output = "";
			for (int i = 1, n = _charArray.Length - 1; i < n; i++) {
				output += _charArray[i];
			}

			return output;
		}

		public static string GetHashSha256(string str) {
			var crypt = new SHA256Managed();
			var hash = new StringBuilder();
			var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetByteCount(str));
		
			foreach (byte theByte in crypto) {
				hash.Append(theByte.ToString("x2"));
			}

			return hash.ToString();
		}

		public static float Lerp(float a, float b, float t) {
			return a + (b - a) * t;
		}

		public static Vector3 Lerp(Vector3 a, Vector3 b, float t) {
			return new Vector3(
				Lerp(a.x, b.x, t), 
				Lerp(a.y, b.y, t), 
				Lerp(a.z, b.z, t)
			);
		}
		
		public static bool NearlyEquals(float a, float b, float delta) {
			return Mathf.Abs(a - b) < delta;
		}

		public static bool NearlyEquals(Vector3 a, Vector3 b, float delta) {
			return Vector3.Distance(a, b) < delta;
		}
	
	}
}