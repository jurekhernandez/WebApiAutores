using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using WebApiAutores.DTOs;
// usuario@corre.com
// |1qaz2WSX
/*
 {
  "email": "usuario@corre.com",
  "password": "|1qaz2WSX"
}
 */
// Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InVzdWFyaW9AY29ycmUuY29tIiwibG8gcXVlIHlvIHF1aWVyYSI6ImN1YWxxdWllciBjb3NhIiwiZXhwIjoxNjY4MTg1ODkzfQ.U2SVaRN_K3gVT7m4vZL8LF4MQ1mUMiiQYkUUi1K4-fE
namespace WebApiAutores.Servicios {
    public class HashService {
        public ResultadoHash Hash(string textoPlano) {
            var sal = new byte[16];
            using(var random = RandomNumberGenerator.Create()) {
                random.GetBytes(sal);
            }
            return Hash(textoPlano, sal);
        }


        public ResultadoHash Hash(string textoPlano, byte[] sal) {
            var llaveDerivada = KeyDerivation.Pbkdf2(password: textoPlano,
                salt: sal, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            var hash = Convert.ToBase64String(llaveDerivada);

            return new ResultadoHash() {
                Hash = hash,
                Sal = sal
            };
        }
    }
}
