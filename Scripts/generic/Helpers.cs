namespace generic {
	using Random = UnityEngine.Random;
	using System;
	using System.Collections.Generic;
	using System.Security.Cryptography;
	using System.Text;
	using UnityEngine;

	public static class Helpers {
		public static string genId() { return System.Guid.NewGuid().ToString("N"); }
		
		public static int hashUtf8Seed(string value) {
			MD5 md5Hasher = MD5.Create();
			byte[] hashedValue = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(value));
			return BitConverter.ToInt32(hashedValue, 0);
		}
		
		public static T randomListValue<T>(List<T> input) {
			int choice = (int)Mathf.Floor(Random.Range(0f, (float)input.Count - 0.001f));
			return input[choice];
		}
	}
}
