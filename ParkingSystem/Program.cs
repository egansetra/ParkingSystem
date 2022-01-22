using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParkingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Parking System");
            Console.WriteLine("1. Create Parking Lot");
            Console.WriteLine("2. Park");
            Console.WriteLine("3. Leave");
            Console.WriteLine("4. Status");
            Console.WriteLine("5. Count Type Vehicle");
            Console.WriteLine("6. Show Registration Number Odd/Even");
            Console.WriteLine("7. Show Registration Number by colour");
            Console.WriteLine("8. Show Slot Number by colour");
            Console.WriteLine("9. Show Slot Number by Registration Number");
            Console.WriteLine("0. Exit");
            
            Console.Write("Option: ");
            string key = Console.ReadLine();
            switch (key)
            {
                case "1":
                    CreateParkingLot();
                    break;
                case "2":
                    Park();
                    break;
                case "3":
                    Leave();
                    break;
                case "4":
                    Status();
                    break;
                case "5":
                    CountTypeVehicle();
                    break;
                case "6":
                    ShowRegistrationNumberOddEvent();
                    break;
                case "7":
                    ShowRegistrationNumberByColour();
                    break;
                case "8":
                    ShowSlotNumberByColour();
                    break;
                case "9":
                    ShowSlotNumberByRegistrationNumber();
                    break;
                case "0":
                    break;
                default:
                    Menu();
                    break;
            }
        }
        public static void CountTypeVehicle()
        {
            Console.Clear();
            Console.Write("Enter Type Vehicle: ");
            string input = Console.ReadLine();
            List<Parking> csv = ReadCSV();
            int count = 0;

            foreach (Parking row in csv)
            {
                if (!string.IsNullOrWhiteSpace(row.Vehicle) && row.Vehicle == input)
                    count++;
            }

            Console.WriteLine(count);
            Console.ReadLine();
            Menu();
        }

        public static void CreateParkingLot()
        {
            Console.Clear();
            Console.WriteLine("Enter Lot Size");
            string value = Console.ReadLine();
            int lotSize = 0;

            if (int.TryParse(value, out lotSize))
            {
                StringBuilder csv = new StringBuilder();
                for (int i = 1; i <= lotSize; i++)
                {
                    csv.AppendLine(String.Format("{0},,,,", i));
                }

                WriteCSV(csv.ToString());

                Menu();
            }
            else
            {
                Console.WriteLine("Please Enter Number Value");
                Console.ReadLine();
                Menu();
            }
        }
        public static void Leave()
        {
            Console.Clear();
            Console.Write("Enter slot number: ");
            StringBuilder sb = new StringBuilder();

            string input = Console.ReadLine();
            string msg = "Please enter valid slot";
            int slot = 0;
            if (int.TryParse(input, out slot))
            {
                List<Parking> csv = ReadCSV();
                foreach (Parking row in csv)
                {
                    if (row.Slot == slot)
                    {
                        sb.AppendLine(String.Format("{0},,,", row.Slot));
                        msg = String.Format("Slot Number {0} is free", row.Slot);
                    }
                    else
                        sb.AppendLine(String.Format("{0},{1},{2},{3}",row.Slot, row.RegNo,row.Color,row.Vehicle));
                }
            }

            WriteCSV(sb.ToString().Trim());
            Console.WriteLine(msg);
            Console.ReadLine();
            Menu();
        }
        public static void Park()
        {
            Console.Clear();
            Console.WriteLine("Enter Vehicle. Format(PlateNumber<space>Colour<space>TypeVehicle)");
            string val = Console.ReadLine();

            StringBuilder sb = new StringBuilder();
            string[] newData = val.Split(new char[0]);
            string msg = "Sorry, Parking lot is full";
            bool isValid = true;
            bool assigned = false;

            if (newData.Length != 3)
            {
                msg = "Format input not valid";
                isValid = false;
            }
            else if (newData.Length == 3)
            {
                string[] regsNo = newData[0].Split('-');

                if (regsNo.Length != 3)
                {
                    msg = "Registration Number not a valid. Please use this format T-000-XX";
                    isValid = false;
                }
                else if (regsNo.Length == 3 && !int.TryParse(regsNo[1], out int value))
                {
                    msg = "Registration Number not a valid. Please use this format T-000-XX";
                    isValid = false;
                }
            }

            if (isValid)
            {
                List<Parking> newCSV = ReadCSV();
                foreach (Parking csv in newCSV)
                {
                    if (csv != null)
                    {
                        if (string.IsNullOrWhiteSpace(csv.RegNo) && !assigned)
                        {
                            sb.AppendLine(String.Format("{0},{1},{2},{3}", csv.Slot, newData[0], newData[1], newData[2]));
                            assigned = true;
                            msg = String.Format("Allocated slot number {0}", csv.Slot);
                        }
                        else
                            sb.AppendLine((String.Format("{0},{1},{2},{3}", csv.Slot, csv.RegNo, csv.Color, csv.Vehicle)));
                    }
                }
            }

            if (assigned)
                WriteCSV(sb.ToString().Trim());

            Console.WriteLine(msg);
            Console.ReadLine();
            Menu();
        }
        public static void Status()
        {
            Console.Clear();
            List<Parking> csv = ReadCSV();
            Console.WriteLine(String.Format("{0,-5}{1,-12}{2,-10}{3,-10}", "Slot", "No.", "Color", "Type"));
            foreach (Parking row in csv)
            {
                if (!String.IsNullOrWhiteSpace(row.RegNo))
                    Console.WriteLine(String.Format("{0,-5}{1,-12}{2,-10}{3,-10}", row.Slot, row.RegNo, row.Color, row.Vehicle));
            }

            Console.WriteLine();
            Console.ReadLine();
            Menu();
        }
        public static void ShowSlotNumberByRegistrationNumber()
        {
            Console.Clear();
            Console.Write("Enter registration number: ");
            string input = Console.ReadLine();
            List<Parking> csv = ReadCSV();
            string msg = "Not Found";

            foreach (Parking row in csv)
            {
                if (!String.IsNullOrWhiteSpace(row.RegNo) && row.RegNo == input)
                {
                    Console.WriteLine(String.Format("{0}", row.Slot));
                    msg = string.Empty;
                }
            }

            Console.WriteLine(msg);
            Console.ReadLine();
            Menu();
        }
        public static void ShowSlotNumberByColour()
        {
            Console.Clear();
            Console.Write("Enter colour: ");
            string input = Console.ReadLine();
            List<Parking> csv = ReadCSV();
            string delim = string.Empty;
            string msg = "Not Found";
            foreach (Parking row in csv)
            {
                if (!string.IsNullOrWhiteSpace(row.RegNo) && input == row.Color)
                {
                    Console.Write(String.Format("{0}{1}", delim, row.Slot));
                    delim = ",";
                    msg = string.Empty;
                }
            }

            Console.WriteLine(msg);
            Console.ReadLine();
            Menu();
        }
        public static void ShowRegistrationNumberByColour()
        {
            Console.Clear();
            Console.Write("Enter Colour: ");
            string input = Console.ReadLine();
            List<Parking> csv = ReadCSV();
            string delim = string.Empty;
            string msg = "Not Found";
            foreach (Parking row in csv)
            {
                if (!string.IsNullOrWhiteSpace(row.Color) && row.Color == input)
                {
                    Console.Write(String.Format("{0}{1}", delim, row.RegNo));
                    delim = ",";
                    msg = string.Empty;
                }
            }

            Console.WriteLine(msg);
            Console.ReadLine();
            Menu();
        }
        public static void ShowRegistrationNumberOddEvent()
        {
            Console.Clear();
            Console.WriteLine("1. Odd Plate");
            Console.WriteLine("2. Even Plate");
            Console.Write("Option: ");
            string input = Console.ReadLine();
            List<Parking> csv = ReadCSV();
            StringBuilder sb = new StringBuilder();
            string delim = string.Empty;
            string msg = "Not Found";
            foreach (Parking row in csv)
            {
                if (!String.IsNullOrWhiteSpace(row.RegNo))
                {
                    string[] plate = row.RegNo.Split('-');
                    if (int.TryParse(plate[1], out int value))
                    {
                        if (input == "1" && value % 2 > 0)
                        {
                            Console.Write(delim + row.RegNo);
                            delim = ",";
                            msg = string.Empty;
                        }
                        else if (input == "2" && value % 2 == 0)
                        {
                            Console.Write(delim + row.RegNo);
                            delim = ",";
                            msg = string.Empty;
                        }
                    }
                }
            }

            Console.WriteLine(msg);
            Console.ReadLine();
            Menu();
        }


        private static void WriteCSV(string csv)
        {
            using (FileStream fs = new FileStream("Parking.csv", FileMode.Create))
            {
                using (StreamWriter wr = new StreamWriter(fs))
                {
                    wr.Write(csv);
                }
            }
        }
        private static List<string> ReadCSV2()
        {
            List<string> ls = new List<string>();
            using (FileStream fs = new FileStream("Parking.csv", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() > 0)
                    {
                        ls.Add(sr.ReadLine());
                    }
                }
            }

            return ls;
        }
        private static List<Parking> ReadCSV()
        {
            List<Parking> ls = new List<Parking>();
            using (FileStream fs = new FileStream("Parking.csv", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() > 0)
                    {
                        string[] row = sr.ReadLine().Split(',');
                        ls.Add(new Parking() { 
                            Slot = Convert.ToInt32(row[0]),
                            RegNo = row[1],
                            Color = row[2],
                            Vehicle = row[3]
                        });
                    }
                }
            }

            return ls;
        }
    }
}
