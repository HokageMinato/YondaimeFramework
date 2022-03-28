using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace YondaimeFramework.EditorHandles
{
    public class DeterministicHashGenerator : CustomBehaviour
    {

        public static int GetHashOf(string value)
        {
            if (value == string.Empty)
                return 0;

            var mystring = value;
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(mystring));
            return BitConverter.ToInt32(hashed, 0);

        }

    }
}