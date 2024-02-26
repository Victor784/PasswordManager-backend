using System.Text;
using System.Security.Cryptography;

namespace PassMngr.Services
{
    public class HashingService
    {
        private List<string> pepperList;
        public HashingService()
        {
            pepperList = new List<string>();

            try
            {
                string filePath = "../pepperList.txt";

                string[] lines = File.ReadAllLines(filePath);

                pepperList.AddRange(lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }

        public string addPepper(string password, string pepper = "")
        {
            string randomPepper;
            if (pepper == "")
            {
                Random random = new Random();
                int randomIndex = random.Next(0, pepperList.Count);

                randomPepper = pepperList[randomIndex];
            }
            else 
            {
                randomPepper = pepper;
            }

            return password + randomPepper;
        }

        public string HashString(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public List<string> getAllPeppers()
        {
            return pepperList;
        }

    }
}
