using System;

namespace SapewinWeb.Metodos
{
    public class CalculosdeHora
    {
        static public int[] Tempo_Minuto(string[] Times) 
        {
            int[] Minutes = new int[Times.Length];

            for (int i = 0; i < Times.Length; i++)
            {
                Minutes[i] = ((Convert.ToInt32(Times[i].Split(':')[0]) * 60) + Convert.ToInt32(Times[i].Split(':')[1]));
            }

            return Minutes;
        }

        static public string[] Minuto_Tempo(int[] Minutes)
        {
            string[] Times = new string[Minutes.Length];           

            for (int i = 0; i < Minutes.Length; i++)
            {
                Times[i] = $"{(Math.Floor(Convert.ToDouble(Minutes[i] / 60))).ToString("00")}:{(Minutes[i] % 60).ToString("00")}";
            }

            return Times;
        }

        static public int SomaHoras_Minutos(string[] Times) 
        {
            int Minutes = 0;

            for (int i = 0; i < Times.Length; i++)
            {
                Minutes = Minutes + ((Convert.ToInt32(Times[i].Split(':')[0]) * 60) + Convert.ToInt32(Times[i].Split(':')[1]));
            }

            return Minutes;
        }

        static public string SomaHoras_Tempo(int[] Minutes)
        {
            int Time = 0;


            for (int i = 0; i < Minutes.Length; i++)
            {
                Time = Time + Minutes[i];                
            }

            return $"{(Math.Floor(Convert.ToDouble(Time / 60))).ToString("00")}:{(Time % 60).ToString("00")}";
        }
        
        static public DateTime RetornaSegundadaSemana()
        {
            int dias = 0;

            while (Convert.ToInt32(DateTime.Now.DayOfWeek) + dias != 1)
            {
                if (Convert.ToInt32(DateTime.Now.DayOfWeek) > 1)
                {
                    dias--;
                }
                else
                {
                    dias++;
                }
            }

            return DateTime.Now.AddDays(dias);
        } 
    }
}