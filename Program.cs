using System;
namespace Erettsegi;

internal class Program
{
    static void Main(string[] args)
    {
        List<byte> dobasok = File.ReadAllText("dobasok.txt").Split().Select(G => Convert.ToByte(G)).ToList();
        List<string> osvenyek = File.ReadAllLines("osvenyek.txt").ToList();

        Console.WriteLine("2. feladat\nA dobások száma: {0}\nAz ösvények száma: {1}", dobasok.Count, osvenyek.Count);
        Console.WriteLine();

        var leghosszabb = osvenyek.Where(G => G.Length == osvenyek.Max(G => G.Length)).First();
        Console.WriteLine("3. feladat\nAz egyik leghosszabb a(z) {0}. ösvény, hossza: {1}", osvenyek.IndexOf(leghosszabb) + 1, leghosszabb.Length);
        Console.WriteLine();

        Console.Write("4. feladat\nAdja meg egy ösvény sorszámát! ");
        int.TryParse(Console.ReadLine(), out int osvenySorszama);
        Console.Write("Adja meg a játékosok számát! ");
        byte.TryParse(Console.ReadLine(), out byte jatekosokSzama);
        Console.WriteLine();
        osvenySorszama--;

        int m = osvenyek[osvenySorszama].Count(G => G == 'M'),
            v = osvenyek[osvenySorszama].Count(G => G == 'V'),
            e = osvenyek[osvenySorszama].Count(G => G == 'E');
        Console.WriteLine("5. feladat");
        if (m > 0) Console.WriteLine("M: {0} darab", m);
        if (v > 0) Console.WriteLine("V: {0} darab", v);
        if (e > 0) Console.WriteLine("E: {0} darab", e);

        var temp = osvenyek[osvenySorszama].Select((G, index) => G != 'M' ? $"{index + 1}\t{G}" : "x").ToList();
        temp.RemoveAll(G => G == "x");
        File.WriteAllLines("../../../kulonleges.txt", temp);

        (int kor, int[] jatekosok) = Jatek(osvenyek[osvenySorszama].Length, jatekosokSzama, dobasok);
        Console.WriteLine("\n7. feladat\nA játék a(z) {0}. körben fejeződött be. A legtávolabb jutó(k) sorszáma: {1}", kor+1, jatekosok.ToList().IndexOf(jatekosok.Where(G => G == osvenyek[osvenySorszama].Length-1).First())+1);
        Console.WriteLine();

        (kor, jatekosok) = Jatek(osvenyek[osvenySorszama], jatekosokSzama, dobasok);
        Console.Write("8. feladat\nNyertes(ek): ");
        int index = 1;
        jatekosok.ToList().ForEach((G) =>
        {
            if (G == osvenyek[osvenySorszama].Length - 1)
                Console.Write(index+++" ");
            else
                index++;
        });
    }

    private static (int, int[]) Jatek(String osveny, byte jatekosokSzama, List<byte> dobasok)
    {
        int[] jatekosok = new int[jatekosokSzama];
        int index = 0, kor = 0;

        foreach(byte dobas in dobasok)
        {
            jatekosok[index] += dobas;
            jatekosok[index] += osveny[index] switch
            {
                'M' => 0,
                'V' => dobas,
                'E' => -dobas,
                _ => throw new Exception("Hibás adat az osvenyek.txt-ben!")
            };

            index++;
            if(index % jatekosokSzama == 0)
            {
                kor++;
                index %= jatekosokSzama;

                if(jatekosok.Any(G => G==osveny.Length-1)) break;
            }
        }

        return (kor, jatekosok);
    }
    private static (int, int[]) Jatek(int osvenyHossza, byte jatekosokSzama, List<byte> dobasok)
    {
        int[] jatekosok = new int[jatekosokSzama];
        int index = 0, kor = 0;

        foreach (byte dobas in dobasok)
        {
            jatekosok[index] += dobas;

            index++;
            if (index % jatekosokSzama == 0)
            {
                kor++;
                index %= jatekosokSzama;

                if (jatekosok.Any(G => G == osvenyHossza-1)) break;
            }
        }

        return (kor, jatekosok);
    }
}