/* LINQ com Lambda
 * Operacoes do LINQ:
 *  • Filtering: Where, OfType;
 *  • Sorting: OrderBy, OrderByDescending, ThenBy, ThenByDescending, Reverse;
 *  • Set: Distinct, Except, Intersect, Union;
 *  • Quantification: All, Any, Contains;
 *  • Projection: Select, SelectMany;
 *  • Partition: Skip, Take;
 *  • Join: Join, GroupJoin;
 *  • Grouping: GroupBy;
 *  • Generational: Empty;
 *  • Equality: SequenceEquals;
 *  • Element: ElementAt, First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault;
 *  • Conversions: AsEnumerable, AsQueryable;
 *  • Concatenation: Concat;
 *  • Aggregation: Aggregate, Average, Count, LongCount, Max, Min, Sum,
 */

/* >>> PROGRAMA PRINCIPAL <<< */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Aula228_LINQ_Lambda.Entities;

namespace Aula228_LINQ_Lambda
{
    class Program
    {
        // Servico para imprimir na tela
        static void Print<T>(string message, IEnumerable<T> collection) // Receve "string" e "IEnumerable" como parametros
        {
            Console.WriteLine(message);
            foreach (T obj in collection)
            {
                Console.WriteLine(obj);
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            Category c1 = new Category() { Id = 1, Name = "Tools", Tier = 2 }; // Cria objeto "c1", tipo "Category"
            Category c2 = new Category() { Id = 2, Name = "Computers", Tier = 1 };
            Category c3 = new Category() { Id = 3, Name = "Electronics", Tier = 1 };

            List<Product> products = new List<Product>() // Cria lista de "Product" - DATA SOURCE
            {
                new Product(){Id = 1, Name = "Computer", Price = 1100.00, Category = c2},
                new Product(){Id = 2, Name = "Hammer", Price = 90.00, Category = c1},
                new Product(){Id = 3, Name = "TV", Price = 1700.00, Category = c3},
                new Product(){Id = 4, Name = "Notebook", Price = 1300.00, Category = c2},
                new Product(){Id = 5, Name = "Saw", Price = 80.00, Category = c1},
                new Product(){Id = 6, Name = "Tablet", Price = 700.00, Category = c2},
                new Product(){Id = 7, Name = "Camera", Price = 700.00, Category = c3},
                new Product(){Id = 8, Name = "Printer", Price = 350.00, Category = c3},
                new Product(){Id = 9, Name = "MacBook", Price = 1800.00, Category = c2},
                new Product(){Id = 10, Name = "Sound Bar", Price = 700.00, Category = c3},
                new Product(){Id = 11, Name = "Level", Price = 70.00, Category = c1}
            };

            //CONSULTAS COM LINQ USANDO EXPRESSOES LAMBDA
            // Onde o Tier da Categoria for igual a 1 e o Preco for menor que 900:
            var r1 = products.Where(p => p.Category.Tier == 1 && p.Price < 900); // Expressao Lambda com "Where"
            Print("TIER 1 AND PRICE < 900.00:", r1); // Imprime - Parametros "string" e "IEnumerable"

            // Onde for categoria "Tools", seleciona o nome:
            var r2 = products.Where(p => p.Category.Name == "Tools").Select(p => p.Name);
            Print("NAME OF PRODUCTIS FROM TOOLS:", r2); // Imprime - Parametros "string" e "IEnumerable"

            // Onde a primeira letra do nome for C, seleciona Nome, Preco e Nome da Categoria, usando ALIAS
            var r3 = products.Where(p => p.Name[0] == 'C').Select(p => new { p.Name, p.Price, CategoryName = p.Category.Name });
            Print("NAMES STARTING WITH 'C' AND ANONYMOUS OBJECT:", r3);

            // Onde o Tier da Categoria for igual a 1, ordena por 
            var r4 = products.Where(p => p.Category.Tier == 1).OrderBy(p => p.Price).ThenBy(p => p.Name);
            Print("TIER 1 ORDERED BY PRICE THEN BY NAME:", r4);

            // Skip e Take - Muito usado para Paginacao
            var r5 = r4.Skip(2).Take(4); // Pula os dois primeiros e adiciona os proximos quatro elementos
            Print("TIER 1 ORDERED BY PRICE THEN BY NAME SKIP 2 TAKE 4:", r5);

            //var r6 = products.First(); // Seleciona o primeiro elemento da lista "products"
            var r6 = products.FirstOrDefault(); // Seleciona o primeiro elemento. Caso seja nulo, nao retorna nada (nem erro)
            Console.WriteLine("FIRST - TEST WITH RESULT: " + r6);

            //var r7 = products.Where(p => p.Price > 3000.00).First(); // ERRO, pois nao ha precos maiores que 3.000,00
            var r7 = products.Where(p => p.Price > 3000.00).FirstOrDefault(); // Evita o erro, retornando o Default (nada)
            Console.WriteLine("FIRST - TEST WITHOUT RESULT: " + r7);

            // Expressao Lambda para retornar um Booleano (um elemento ou nenhum elemento)
            //var r8 = products.Where(p => p.Id == 3).Single(); 
            var r8 = products.Where(p => p.Id == 3).SingleOrDefault(); // O Id do produto e unico, portanto, so pode retornar Bool        Console.WriteLine("SINGLE OR DEFAULT - TEST WITH RESULT: " + r8);

            // Retornar nenhum elemento
            var r9 = products.Where(p => p.Id == 30).SingleOrDefault(); // O Id e unico, portanto, so pode um Booleano como retorno
            Console.WriteLine("SINGLE OR DEFAULT - TEST WITH NO RESULT: " + r9);
            Console.WriteLine();

            // FUNCOES DE AGREGACAO
            // Retorna o maior valor, baseado em "Price"
            var r10 = products.Max(p => p.Price);
            Console.WriteLine("MAX PRICE: " + r10.ToString("F2", CultureInfo.InvariantCulture));

            // Retorna o menor valor, baseado em "Price"
            var r11 = products.Min(p => p.Price);
            Console.WriteLine("MIN PRICE: " + r11.ToString("F2", CultureInfo.InvariantCulture));

            // Retorna a soma dos precos dos produtos da Categoria 1
            var r12 = products.Where(p => p.Category.Id == 1).Sum(p => p.Price);
            Console.WriteLine("CATEGORY 1 PRICES SUM: " + r12.ToString("F2", CultureInfo.InvariantCulture));

            //Retorna a media dos produtos da Categoria 1
            var r13 = products.Where(p => p.Category.Id == 1).Average(p => p.Price);
            Console.WriteLine("CATEGORY 1 AVERAGE PRICE: " + r13.ToString("F2", CultureInfo.InvariantCulture));

            // Retornar 0.0 se nao houver precos nos produtos da Categoria (no caso, categoria 5, que nao existe no Source)
            var r14 = products.Where(p => p.Category.Id == 5).Select(p => p.Price).DefaultIfEmpty(0.0).Average();
            Console.WriteLine("CATEGORY 5 AVERAGE PRICE: " + r14.ToString("F2", CultureInfo.InvariantCulture));

            // OPERACAO AGGREGATE - MUITO IMPORTANTE (REDUCE OU MAP REDUCE)
            var r15 = products.Where(p => p.Category.Id == 1).Select(p => p.Price).Aggregate((x, y) => x + y);
            Console.WriteLine("CATEGORY 1 PRICES SUM WITH AGGREGATE: " + r15.ToString("F2", CultureInfo.InvariantCulture));

            // Retornar 0.0, caso ha haja precos nos produtos da Categoria 1
            var r16 = products.Where(p => p.Category.Id == 5).Select(p => p.Price).Aggregate(0.0, (x, y) => x + y);
            Console.WriteLine("CATEGORY 5 PRICES SUM WITH AGGREGATE: " + r16.ToString("F2", CultureInfo.InvariantCulture));

            Console.WriteLine();

            // Agrupamento por Categoria
            var r17 = products.GroupBy(p => p.Category);
            foreach(IGrouping<Category,Product> group in r17)
            {
                Console.WriteLine("CATEGORY: " + group.Key.Name + ":");
                foreach(Product p in group)
                {
                    Console.WriteLine(p);
                }
            }
        }
    }
}
