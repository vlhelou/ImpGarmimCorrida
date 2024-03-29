﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Globalization;


namespace ImportacaoGPX
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("quantidade de paramametros errado");
                return;
            }
            Stopwatch tempo = new Stopwatch();
            tempo.Start();
            if (Directory.Exists(args[0]))
            {

                List<String> Lista = new List<string>();
                foreach (string arquivo in Directory.GetFiles(args[0], "*.tcx", SearchOption.TopDirectoryOnly))
                {
                    Lista.Add(arquivo);
                }
                foreach (string arquivo in Directory.GetFiles(args[0], "*.gpx", SearchOption.TopDirectoryOnly))
                {
                    Lista.Add(arquivo);
                }

                foreach (string arquivo in Lista)
                {
                    ImportaArquivo(arquivo);
                }
            }
            else if (File.Exists(args[0]))
            {
                ImportaArquivo(args[0]);
            }
            else
            {
                Console.WriteLine("Não existe nem diretório e nem arquivo");
            }
            tempo.Stop();
            Console.WriteLine($"fim em {tempo.Elapsed.ToString()}");
        }

        static void ImportaArquivo(string arquivo)
        {
            if (File.Exists(arquivo))
            {
                Stopwatch TImportacao = new Stopwatch();
                TImportacao.Start();
                FileInfo fi = new FileInfo(arquivo);
                Corrida corrida;
                if (fi.Extension.ToLower() == ".gpx")
                {
                    corrida = ImportaGPX(fi);
                }
                else
                {
                    corrida = ImportaTCX(fi);
                }
                Grava(corrida);
                TImportacao.Stop();
                Console.WriteLine($"a corrida de inicio:{corrida.Inicio} foi importada em: {TImportacao.Elapsed.ToString()}");
            }
            else
            {
                Console.WriteLine($"O arquivo:{arquivo} não existe");
            }

        }

        static void Grava(Corrida corrida)
        {
            using (var db = new DB())
            {

                if (db.Corrida.Where(predicate => predicate.Inicio == corrida.Inicio).Count() == 0)
                {
                    corrida.Tracks = corrida.Tracks.OrderBy(p => p.Hora).ToList();
                    db.Add(corrida);

                    // db.Entry(corrida).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    // db.SaveChanges();
                    // foreach (Track track in corrida.Tracks)
                    // {
                    //     track.IdCorrida = corrida.Id;
                    //     db.Entry(track).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    // }
                    db.SaveChanges();

                }
                else
                {
                    Console.WriteLine("Arquivo já importado");
                }
            }
        }
        static Corrida ImportaGPX(FileInfo arquivo)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(arquivo.FullName);
            XmlNode Root = doc.DocumentElement;
            XmlNode Meta = Root["metadata"];
            Corrida corrida = new Corrida();
            corrida.Tracks = new List<Track>();
            corrida.Inicio = DateTime.Parse(Meta["time"].InnerText);
            XmlNodeList trks = doc.GetElementsByTagName("trkpt");
            int ct = 0;
            foreach (XmlNode ponto in trks)
            {
                ct++;
                Track track = new Track();
                track.Id = ct;
                track.lat = float.Parse(ponto.Attributes["lat"].Value);
                track.lon = float.Parse(ponto.Attributes["lon"].Value);
                track.Elevacao = float.Parse(ponto["ele"].InnerText);
                track.Hora = DateTime.Parse(ponto["time"].InnerText);
                track.Batimento = int.Parse(ponto["extensions"]["ns3:TrackPointExtension"]["ns3:hr"].InnerText);
                track.CadenciaPasso = int.Parse(ponto["extensions"]["ns3:TrackPointExtension"]["ns3:cad"].InnerText);
                corrida.Tracks.Add(track);
            }

            return corrida;
        }

        static Corrida ImportaTCX(FileInfo arquivo)
        {
            Corrida corrida = new Corrida();
            corrida.Tracks = new List<Track>();
            XmlDocument doc = new XmlDocument();
            doc.Load(arquivo.FullName);
            XmlNode Root = doc.DocumentElement;
            corrida.Inicio = DateTime.Parse(Root["Activities"]["Activity"]["Id"].InnerText);
            XmlNodeList trks = doc.GetElementsByTagName("Trackpoint");
            int ct = 0;
            foreach (XmlNode ponto in trks)
            {
                ct++;
                Track track = new Track();
                track.Id = ct;
                if (ponto["Position"] != null)
                {
                    if (float.TryParse(ponto["Position"]["LatitudeDegrees"].InnerText, NumberStyles.Any, new CultureInfo("en-US"), out float lat))
                    {
                        track.lat = lat;
                    }

                    if (float.TryParse(ponto["Position"]["LongitudeDegrees"].InnerText, NumberStyles.Any, new CultureInfo("en-US"), out float lon))
                    {
                        track.lon = lon;
                    }


                }
                if (ponto["AltitudeMeters"] != null)
                {
                    if (float.TryParse(ponto["AltitudeMeters"].InnerText, NumberStyles.Any, new CultureInfo("en-US"), out float altidute))
                    {
                        track.Elevacao = altidute;
                    }
                }

                track.Hora = DateTime.Parse(ponto["Time"].InnerText);
                track.Batimento = int.Parse(ponto["HeartRateBpm"]["Value"].InnerText);
                track.CadenciaPasso = int.Parse(ponto["Extensions"]["ns3:TPX"]["ns3:RunCadence"].InnerText);
                //track.Distancia = decimal.Parse(ponto["DistanceMeters"].InnerText);
                if (decimal.TryParse(ponto["DistanceMeters"].InnerText, NumberStyles.Any, new CultureInfo("en-US"), out decimal distancia))
                {
                    track.Distancia = distancia;
                }
                corrida.Tracks.Add(track);
            }


            return corrida;
        }
    }
}