﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Samochody
{
    class Program
    {
        static void Main(string[] args)
        {
            var samochody = WczytywanieSamochodu("paliwo.csv");
            var producenci = WczytywanieProducenci("producent.csv");

            var zapytanie = from samochod in samochody
                            join producent in producenci
                                on new { samochod.Producent, samochod.Rok }
                                equals new { Producent = producent.Nazwa, producent.Rok }
                            orderby samochod.SpalanieAutostrada descending, samochod.Producent ascending
                            select new
                            {
                                producent.Siedziba,
                                samochod.Producent,
                                samochod.Model,
                                samochod.SpalanieAutostrada
                            };

            var zapytanie2 = samochody.Join(producenci,
                                            s => new { s.Producent, s.Rok },
                                            p => new { Producent = p.Nazwa, p.Rok },
                                            (s, p) => new
                                            {
                                                Samochod = s,
                                                Producent = p
                                            })
                                        .OrderByDescending(s => s.Samochod.SpalanieAutostrada)
                                        .ThenBy(s => s.Samochod.Producent);

            foreach (var samochod in zapytanie2.Take(10))
            {
                Console.WriteLine(samochod.Producent.Siedziba + " " + samochod.Samochod.Producent + " " + samochod.Samochod.Model + " : " + samochod.Samochod.SpalanieAutostrada);
            }
        }

        private static List<Samochod> WczytywanieSamochodu(string sciezka)
        {
            var zapytanie = File.ReadAllLines(sciezka)
                                .Skip(1)
                                .Where(l => l.Length > 1)
                                .WSamochod();

            return zapytanie.ToList();
        }


        private static List<Producent> WczytywanieProducenci(string sciezka)
        {
            var zapytanie = File.ReadAllLines(sciezka)
                                .Where(l => l.Length > 1)
                                .Select(l =>
                                {
                                    var kolumny = l.Split(',');
                                    return new Producent
                                    {
                                        Nazwa = kolumny[0],
                                        Siedziba = kolumny[1],
                                        Rok = int.Parse(kolumny[2])
                                    };
                                });

            return zapytanie.ToList();
        }
    }

    public static class SamochodRozszerzenie
    {
        public static IEnumerable<Samochod> WSamochod(this IEnumerable<string> zrodlo)
        {
            foreach (var linia in zrodlo)
            {
                var kolumny = linia.Split(',');

                yield return new Samochod
                {
                    Rok = int.Parse(kolumny[0]),
                    Producent = kolumny[1],
                    Model = kolumny[2],
                    Pojemnosc = double.Parse(kolumny[3], CultureInfo.InvariantCulture),
                    IloscCylindrow = int.Parse(kolumny[4]),
                    SpalanieMiasto = int.Parse(kolumny[5]),
                    SpalanieAutostrada = int.Parse(kolumny[6]),
                    SpalanieMieszane = int.Parse(kolumny[7])
                };
            }
        }
    }
}