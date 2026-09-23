using System.IO;
using System.Text;

namespace examPM01
{
    public class CityControl
    {
        public City[] Cities { get; }

        public CityControl(int size)
        {
            Cities = new City[size];
            for (int i = 0; i < size; i++)
                Cities[i] = new City();
        }


        public void Sort()
        {
            for (int i = 0; i < Cities.Length; i++)
            {
                for (int j = 0; j < Cities.Length - 1; j++)
                {
                    if (Cities[j].Population == Cities[j + 1].Population)
                    {
                        if (Cities[j].FoundationDate.CompareTo(Cities[j + 1].FoundationDate) < 0)
                        {
                            City temp = Cities[j];
                            Cities[j] = Cities[j + 1];
                            Cities[j + 1] = temp;
                        }
                    }
                    else if (Cities[j].Population < Cities[j + 1].Population)
                    {
                        City temp = Cities[j];
                        Cities[j] = Cities[j + 1];
                        Cities[j + 1] = temp;
                    }
                }
            }
        }

        public void SaveToFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            for (int i = 0; i < Cities.Length; i++)
            {
                string line = Cities[i].Name + " " + Cities[i].FoundationDate + " " + Cities[i].Population + "\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(line);
                fs.Write(bytes, 0, bytes.Length);
            }

            fs.Close();
        }

    }
}
