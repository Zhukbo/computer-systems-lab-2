using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введіть перше число: ");
            string numb1 = Console.ReadLine();
            Console.Write("Введіть друге число: ");
            string numb2 = Console.ReadLine();

            int mult1, mult2;
            if (Int32.TryParse(numb1, out mult1) && Int32.TryParse(numb2, out mult2))
            {
                ButsAlgorithm(mult1, mult2);
            }
            else
            {
                Console.WriteLine("Ви ввели щось погане");
            }

            Console.ReadKey();
        }

        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        static void ButsAlgorithm(int numb1, int numb2)
        {
            int m = numb1, r = numb2;

            // numb1
            List<int> a = new List<int>();
            // -numb1
            List<int> s = new List<int>();
            // numb2
            List<int> p = new List<int>();

            Get2PowKBitsNumb(m, r, out a, out p, out s);

            int x = a.Count;
            int y = p.Count;

            ComplementForList(a, y + 1);
            ComplementForList(s, y + 1);
            ComplementForListForP(p, x);
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"A:             \tРезультат: {GetStrFromList(a)}");
            Console.WriteLine($"S:             \tРезультат: {GetStrFromList(s)}");
            Console.WriteLine($"P:             \tРезультат: {GetStrFromList(p)}");
            Console.WriteLine("-------------------------------------------------");

            for (int i = 0; i < y; i++)
            {
                if (p[p.Count - 1] == 1 && p[p.Count - 2] == 0)
                {
                    AddBinary(p, a);
                    Console.WriteLine($"Бінарне додавання P & A \tРезультат: {GetStrFromList(p)}");
                }
                else if (p[p.Count - 1] == 0 && p[p.Count - 2] == 1)
                {
                    AddBinary(p, s);
                    Console.WriteLine($"Бінарне додавання P & S \tРезультат: {GetStrFromList(p)}");
                }
                RightShift(p);
                Console.WriteLine($"Зсув вправо \tРезультат: {GetStrFromList(p)}");
            }
            p.RemoveAt(p.Count - 1);
            Console.WriteLine($"Відкидання P \t\tРезультат: {GetStrFromList(p)}");
            int result = GetNumbFromTwoComplement(p);
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Результат в десятковій: {result}");
        }

        private static void ComplementForListForP(List<int> p, int x)
        {
            List<int> helper = new List<int>() { 0 };
            while (x > 0)
            {
                helper.AddRange(p);
                p.Clear();
                p.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
                x--;
            }
            p.Add(0);
        }

        static int GetNumbFromTwoComplement(List<int> p)
        {
            int result = -p[0] * (int)Math.Pow(2, p.Count - 1);
            for (int i = p.Count - 1; i > 0; i--)
            {
                result += p[i] * (int)Math.Pow(2, p.Count - i - 1);
            }
            return result;
        }
        static string GetStrFromList(List<int> list)
        {
            string result = "";
            foreach (int item in list)
            {
                result += item;
            }
            return result;
        }
        static void AddBinary(List<int> p, List<int> aOrs)
        {
            int mind = 0;
            for (int i = p.Count - 1; i >= 0; i--)
            {
                if ((p[i] == 0 && aOrs[i] == 1 && mind == 0) || (p[i] == 1 && aOrs[i] == 0 && mind == 0) || (p[i] == 0 && aOrs[i] == 0 && mind == 1))
                {
                    p[i] = 1;
                    mind = 0;
                }
                else if ((p[i] == 0 && aOrs[i] == 1 && mind == 1) || (p[i] == 1 && aOrs[i] == 0 && mind == 1) || (p[i] == 1 && aOrs[i] == 1 && mind == 0))
                {
                    p[i] = 0;
                    mind = 1;
                }
                else if (p[i] == 0 && aOrs[i] == 0 && mind == 0)
                {
                    p[i] = 0;
                    mind = 0;
                }
                else
                {
                    p[i] = 1;
                    mind = 1;
                }
            }
        }
        static void RightShift(List<int> p)
        {
            for (int i = p.Count - 1; i > 0; i--)
            {
                p[i] = p[i - 1];
            }
        }
        static void ComplementForList(List<int> bits, int count)
        {
            while (count > 0)
            {
                bits.Add(0);
                count--;
            }
        }
        static List<int> GetBitsFromNumb(int numb)
        {
            List<int> bits = new List<int>();
            if (numb > 0)
            {
                while (numb != 0)
                {
                    bits.Add(numb % 2);
                    numb = numb / 2;
                }
                bits.Reverse();
            }
            else
            {
                bits = GetBitsFromNumb(-numb);
                Inversion(bits);
                AddOne(bits);
                List<int> addOne = new List<int>() { 1 };
                addOne.AddRange(bits);
                return addOne;
            }
            return bits;
        }
        static void Get2PowKBitsNumb(int numb1, int numb2, out List<int> first, out List<int> second, out List<int> invertFirst)
        {
            first = GetBitsFromNumb(numb1);
            second = GetBitsFromNumb(numb2);
            invertFirst = GetBitsFromNumb(-numb1);

            int maxCount = second.Count;
            if (invertFirst.Count > first.Count && invertFirst.Count > second.Count)
            {
                maxCount = invertFirst.Count;
            }
            else if (first.Count > second.Count)
            {
                maxCount = first.Count;
            }
            int lenght = (int)Math.Log(maxCount, 2) + 1;
            lenght = (int)Math.Pow(2, lenght);

            int elem1 = (numb1 < 0) ? 1 : 0;
            List<int> helper = new List<int>() { elem1 };
            int countOfElem = first.Count;
            for (int i = 0; i < lenght - countOfElem; i++)
            {
                helper.AddRange(first);
                first.Clear();
                first.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
            }
            int elem2 = (numb2 < 0) ? 1 : 0;
            helper = new List<int>() { elem2 };
            countOfElem = second.Count;
            for (int i = 0; i < lenght - countOfElem; i++)
            {
                helper.AddRange(second);
                second.Clear();
                second.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
            }
            int elem3 = ((-numb1) < 0) ? 1 : 0;
            countOfElem = invertFirst.Count;
            helper = new List<int>() { elem3 };
            for (int i = 0; i < lenght - countOfElem; i++)
            {
                helper.AddRange(invertFirst);
                invertFirst.Clear();
                invertFirst.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
            }
        }
        static void Inversion(List<int> bits)
        {
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i] == 0)
                {
                    bits[i] = 1;
                }
                else
                {
                    bits[i] = 0;
                }
            }
        }
        static List<int> AddOne(List<int> bits)
        {
            int mind = 1;
            for (int i = bits.Count - 1; i >= 0; i--)
            {
                if (i == 0 && bits[i] == 1 && mind == 1)
                {
                    List<int> newBits = new List<int>();
                    newBits.AddRange(new List<int>() { 1, 1 });
                    newBits.AddRange(bits);
                    return newBits;
                }
                else if (bits[i] == 0 && mind == 1)
                {
                    bits[i] = 1;
                    mind = 0;
                }
                else if (bits[i] == 1 && mind == 1)
                {
                    bits[i] = 0;
                }
            }
            return bits;
        }
    }
}
