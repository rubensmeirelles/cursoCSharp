﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    public partial class PopulaProdutos : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values('Coca-Cola Diet', 'Refrigerante de 350 ml', 5.45,  'cocacola.jpg', 50, now(), 1)");
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values('Lanche de Atum', 'Lanche de Atum com maionese', 8.50,  'atum.jpg', 10, now(), 2)");
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values('Pudim', 'Pudim de leite condensado', 6.75,  'pudim.jpg', 20, now(), 3)");
        }
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete fromm produtos");
        }
    }
}
