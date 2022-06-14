using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Autoservice autoservice = new Autoservice();
            autoservice.Work();
        }
    }

    class Autoservice
    {
        private int _money;
        private Queue<Client> _clients = new Queue<Client>();
        private Storage _storage;

        public Autoservice()
        {
            _money = CreateMoney();
            _storage = new Storage();
        }

        public void Work()
        {
            CreateQueue();
            int index = 1;

            foreach (var client in _clients)
            {
                Console.WriteLine($"Клиент {index}.");
                client.ShowInfo();
                index++;
                Console.WriteLine($"Баланс мастерской: {_money}");
                Console.WriteLine("1. Взяться за работу");
                Console.WriteLine("2. Отказать");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Repair(client);
                        break;
                    case "2":
                        DenyClient();
                        break;
                    default:
                        Console.WriteLine("Клиент не понял что вы решили по его проблеме и уехал");
                        break;
                }
            }
        }

        private void DenyClient()
        {
            int penalty = 100;
            Console.WriteLine($"Вы отказали кленту, штраф {penalty}");
            _money -= penalty;
        }

        private void Repair(Client client)
        {
            int penalty = 100;

            if (_storage.TryRepairCar(client) == true)
            {
                Console.WriteLine("Вы починили поломку");
                _money += _storage.CalculatePrice(client);
            }
            else 
            {
                Console.WriteLine("Вы не починили поломку");
                Console.WriteLine($"Штраф {penalty}");
                _money -= penalty;
            }
        }

        private int CreateMoney()
        {
            Random random = new Random();
            int minMoney = 500;
            int maxMoney = 1000;
            int money = random.Next(minMoney, maxMoney);
            return money;
        }

        private void CreateQueue()
        {
            Random random = new Random();
            int minClientsQantity = 2;
            int maxClientsQantity = 6;
            int index = random.Next(minClientsQantity, maxClientsQantity);

            for (int i = 0; i < index; i++)
            {
                _clients.Enqueue(new Client());
            }
        }
    }

    class Storage 
    {
        private List<Detail> _details = new List<Detail>();
        private List<Detail> _detailsForClients = new List<Detail>();

        public Storage()
        {
            CreateDetails();
            CreateDetailsForClients();
        }

        public bool TryRepairCar(Client client)
        {
            ShowInfo();
            Console.WriteLine("Выберите делать для починки");
            string input = Console.ReadLine();
            int number;

            if (int.TryParse(input, out number))
            {
                if (number > 0 && number <= _detailsForClients.Count)
                {
                    if (client.Problem == _detailsForClients[number-1].DetailName)
                    {
                        _detailsForClients.RemoveAt(number-1);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Вы установили не ту деталь");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int CalculatePrice(Client client)
        {
            int workPirce = 50;
            int price = 0;

            foreach (var detail in _details)
            {
                if (client.Problem == detail.DetailName)
                {
                    price += detail.DetailCoast + workPirce;
                    break;
                }
            }

            return price;
        }

        private void CreateDetailsForClients()
        {
            Random random = new Random();
            int minDetails = 2;
            int maxDetails = 5;
            int amount = random.Next(minDetails, maxDetails);

            for (int i = 0; i < amount; i++)
            {
                int index = random.Next(0, _details.Count);
                _detailsForClients.Add(GetDetail(index));
            }
        }

        private void ShowInfo()
        {
            Console.WriteLine("На складе:");

            for (int i = 0; i < _detailsForClients.Count; i++)
            {
                Console.WriteLine($"{i+1} {_detailsForClients[i].DetailName} стоимость {_detailsForClients[i].DetailCoast}");
            }
        }

        private Detail GetDetail(int index)
        {
            return _details[index];
        }

        private void CreateDetails()
        {
            _details.Add(new Detail("Двигатель", 150));
            _details.Add(new Detail("Подвеска", 100));
            _details.Add(new Detail("Шины", 70));
            _details.Add(new Detail("Фары", 50));
            _details.Add(new Detail("Электроника", 25));
            _details.Add(new Detail("Ёлочка-вонючка", 10));
        }
    }

    class Client
    {
        private List<string> _problems = new List<string>();
        public string Problem { get; private set; }

        public Client()
        {
            CreateProblems();
            ChooseProblem();
        }

        public void ShowInfo()
        {
            Console.WriteLine(Problem);
        }

        private void ChooseProblem()
        {
            Random random = new Random();
            int index = random.Next(1, _problems.Count);
            Problem = _problems[index];
        }

        private void CreateProblems()
        {
            _problems.Add(new string("Двигатель"));
            _problems.Add(new string("Подвеска"));
            _problems.Add(new string("Шины"));
            _problems.Add(new string("Фары"));
            _problems.Add(new string("Электроника"));
            _problems.Add(new string("Ёлочка-вонючка"));
        }
    }

    class Detail
    { 
        public string DetailName { get; private set; }
        public int DetailCoast { get; private set; }

        public Detail(string detailName, int detailCoast)
        {
            DetailName = detailName;
            DetailCoast = detailCoast;
        }
    }
}