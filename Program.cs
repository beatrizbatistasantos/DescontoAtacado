using System;
using System.Collections.Generic;
using System.Globalization;

class Produto
{
    public string Gtin;
    public string Nome;
    public double PrecoVarejo;
    public double? PrecoAtacado;
    public int? QuantidadeMinima;

    public Produto(string gtin, string nome, double precoVarejo, double? precoAtacado = null, int? quantidadeMinima = null)
    {
        Gtin = gtin;
        Nome = nome;
        PrecoVarejo = precoVarejo;
        PrecoAtacado = precoAtacado;
        QuantidadeMinima = quantidadeMinima;
    }
}

class ItemCompra
{
    public string Gtin;
    public int Quantidade;
    public double ValorPago;

    public ItemCompra(string gtin, int quantidade, double valorPago)
    {
        Gtin = gtin;
        Quantidade = quantidade;
        ValorPago = valorPago;
    }
}

class ProgramaDesconto
{   
    static void Main()
    {
        var cultura = CultureInfo.GetCultureInfo("pt-BR");

        // Lista de produtos disponíveis no catálogo
        Dictionary<string, Produto> catalogo = new()
        {
            { "7891024110348", new Produto("7891024110348", "Sabonete Palmolive", 2.88, 2.51, 12) },
            { "7891048038017", new Produto("7891048038017", "Chá Camomila Dr. Oetker", 4.40, 4.37, 3) },
            { "7896066334509", new Produto("7896066334509", "Torrada Wickbold", 5.19) },
            { "7891700203142", new Produto("7891700203142", "Soja Maçã Ades", 2.39, 2.38, 6) },
            { "7894321711263", new Produto("7894321711263", "Toddy Achocolatado", 9.79) },
            { "7896001250611", new Produto("7896001250611", "Adoçante Linea", 9.89, 9.10, 10) },
            { "7793306013029", new Produto("7793306013029", "Cereal Sucrilhos", 12.79, 12.35, 3) },
            { "7896004400914", new Produto("7896004400914", "Coco Ralado Sococo", 4.20, 4.05, 6) },
            { "7898080640017", new Produto("7898080640017", "Leite Italac 1L", 6.99, 6.89, 12) },
            { "7891025301516", new Produto("7891025301516", "Danoninho Morango", 12.99) },
            { "7891030003115", new Produto("7891030003115", "Creme de Leite Mococa", 3.12, 3.09, 4) },
        };

        // Compras feitas pelo cliente no caixa
        List<ItemCompra> itensVendidos = new()
        {
            new ItemCompra("7891048038017", 1, 4.40),
            new ItemCompra("7896004400914", 4, 16.80),
            new ItemCompra("7891030003115", 1, 3.12),
            new ItemCompra("7891024110348", 6, 17.28),
            new ItemCompra("7898080640017", 24, 167.76),
            new ItemCompra("7896004400914", 8, 33.60),
            new ItemCompra("7891700203142", 8, 19.12),
            new ItemCompra("7891048038017", 1, 4.40),
            new ItemCompra("7793306013029", 3, 38.37),
            new ItemCompra("7896066334509", 2, 10.38),
        };

        Dictionary<string, int> totalPorProduto = new();
        double subtotal = 0.0;

        // Agrupar quantidades por produto e calcular subtotal
        foreach (var item in itensVendidos)
        {
            subtotal += item.ValorPago;

            if (!totalPorProduto.ContainsKey(item.Gtin))
                totalPorProduto[item.Gtin] = 0;

            totalPorProduto[item.Gtin] += item.Quantidade;
        }

        // Verificar descontos
        Dictionary<string, double> descontos = new();
        foreach (var gtin in totalPorProduto.Keys)
        {
            var produto = catalogo[gtin];
            int qtdeComprada = totalPorProduto[gtin];

            if (produto.PrecoAtacado.HasValue && produto.QuantidadeMinima.HasValue)
            {
                if (qtdeComprada >= produto.QuantidadeMinima)
                {
                    double totalNormal = qtdeComprada * produto.PrecoVarejo;
                    double totalComDesconto = qtdeComprada * produto.PrecoAtacado.Value;
                    double valorDesconto = totalNormal - totalComDesconto;

                    if (valorDesconto >= 0.01)
                        descontos[gtin] = Math.Round(valorDesconto, 2);
                }
            }
        }

        // Imprimir resultado final
        double totalDescontos = 0;
        Console.WriteLine("--- Desconto no Atacado ---\n");
        Console.WriteLine(@"Seja muito bem-vindo ao BSuperMarket!
                        Obrigada por nos escolher!!!");

        if (descontos.Count > 0)
        {
            Console.WriteLine("Descontos por produto:");
            foreach (var desc in descontos)
            {
                Console.WriteLine($"{desc.Key,-15} R$ {desc.Value.ToString("0.00", cultura)}");
                totalDescontos += desc.Value;
            }
        }
        else
        {
            Console.WriteLine("Nenhum desconto foi aplicado.");
        }

        Console.WriteLine($"\n(+) Subtotal  =    R$ {subtotal.ToString("0.00", cultura)}");
        Console.WriteLine($"(-) Descontos =      R$ {totalDescontos.ToString("0.00", cultura)}");
        Console.WriteLine($"(=) Total     =    R$ {(subtotal - totalDescontos).ToString("0.00", cultura)}");
        Console.WriteLine("Pressione qualquer tecla para encerrar o programa...");
        Console.ReadKey();
    }

}
