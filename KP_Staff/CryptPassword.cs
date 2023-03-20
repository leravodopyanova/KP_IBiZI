using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KP
{
    public class CryptPassword
    {
        // Инициализирует новую объект данного класса.
        public CryptPassword()
        {
            //задает размер соли и итераций для хэширования по умолчанию.
            HashIterations = 100000;
            SaltSize = 34;
        }

        // Получает или задает количество итераций для хэширования. 
        public int HashIterations
        { get; set; }

        // Получает или устанавливает размер соли, который сгенерируется, еслт не будет задана соль.
        public int SaltSize
        { get; set; }

        // Получает или устанавливает обычный текст для хэширования.
        public string PlainText
        { get; set; }

        // Получает закодированную из 64 символов строку хэшированного текста PlainText.
        public string HashedText
        { get; private set; }

        // Получет или устанавливает соль, которая будет использована в вычислении хэшированного текста HashedText. Это содержит и соль Salt, и количество итераций хэша HashIterations.
        public string Salt
        { get; set; }

        // Вычисляет хэш
        public string Compute()
        {
            if (string.IsNullOrEmpty(PlainText)) throw new InvalidOperationException("Пароль не может быть пустым");

            //если соли нет, то она сгенерируется
            if (string.IsNullOrEmpty(Salt))
                GenerateSalt();

            HashedText = calculateHash(HashIterations);

            return HashedText;
        }

        // Вычисляет хэш, используя соль, сгенерированную по умолчанию. Сгенерирует соль, если ничего не будет назначено.
        public string Compute(string textToHash)
        {
            PlainText = textToHash;
            //ввычислет хэш
            Compute();
            return HashedText;
        }

        // Вычисляет хэш, который также генерирует соль из параметров.
        public string Compute(string textToHash, int saltSize, int hashIterations)
        {
            PlainText = textToHash;
            //вычисляет соль
            GenerateSalt(hashIterations, saltSize);
            //вычисляет хэш
            Compute();
            return HashedText;
        }

        // Расчет хэша, который утилизирует полученную соль.
        public string Compute(string textToHash, string salt)
        {
            PlainText = textToHash;
            Salt = salt;
            // Расширение соли.
            expandSalt();
            Compute();
            return HashedText;
        }

        // Генерация соли с размером соли и количеством итерации по умолчанию.
        public string GenerateSalt()
        {
            if (SaltSize < 1) throw new InvalidOperationException(string.Format("Невозможно сгенерировать соль размером {0}, используйте значение больше 1, рекомендовано: 16", SaltSize));

            var rand = RandomNumberGenerator.Create();

            var ret = new byte[SaltSize];

            rand.GetBytes(ret);

            //преобразует сгенерированную соль к формату итерации.соль
            Salt = string.Format("{0}.{1}", HashIterations, Convert.ToBase64String(ret));

            return Salt;
        }

        // Генерация соли.
        public string GenerateSalt(int hashIterations, int saltSize)
        {
            HashIterations = hashIterations;
            SaltSize = saltSize;
            return GenerateSalt();
        }

        // Получает время в милисекундах, за которое выполнятеся хэширование на каждой итерации.
        public int GetElapsedTimeForIteration(int iteration)
        {
            var sw = new Stopwatch();
            sw.Start();
            calculateHash(iteration);
            return (int)sw.ElapsedMilliseconds;
        }


        private string calculateHash(int iteration)
        {
            // Конвертирует соль в байтовый массив.
            byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);

            var pbkdf2 = new Rfc2898DeriveBytes(PlainText, saltBytes, iteration);
            var key = pbkdf2.GetBytes(64);
            return Convert.ToBase64String(key);
        }

        private void expandSalt()
        {
            try
            {
                // Получает позицию точки, которая разделяет строку.
                var i = Salt.IndexOf('.');

                // Получает хэш по итерации с первого индекса.
                HashIterations = int.Parse(Salt.Substring(0, i), System.Globalization.NumberStyles.Number);

            }
            catch (Exception)
            {
                throw new FormatException("Соль получена не в ожидаемом формате {int}.{string}");
            }
        }

        // Сравнивает хэши пароля на равенство. Использует метод сравнения с постоянным временем.
        public bool Compare(string passwordHash1, string passwordHash2)
        {
            if (passwordHash1 == null || passwordHash2 == null)
                return false;

            int min_length = Math.Min(passwordHash1.Length, passwordHash2.Length);
            int result = 0;

            for (int i = 0; i < min_length; i++)
                result |= passwordHash1[i] ^ passwordHash2[i];

            return 0 == result;
        }
    }
}
