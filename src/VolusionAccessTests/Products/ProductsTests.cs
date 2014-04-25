﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LINQtoCSV;
using NUnit.Framework;
using VolusionAccess;
using VolusionAccess.Models.Configuration;
using VolusionAccess.Models.Product;

namespace VolusionAccessTests.Products
{
	[ TestFixture ]
	public class ProductsTests
	{
		private readonly IVolusionFactory VolusionFactory = new VolusionFactory();
		private VolusionConfig Config;

		[ SetUp ]
		public void Init()
		{
			const string credentialsFilePath = @"..\..\Files\VolusionCredentials.csv";

			var cc = new CsvContext();
			var testConfig = cc.Read< TestConfig >( credentialsFilePath, new CsvFileDescription { FirstLineHasColumnNames = true } ).FirstOrDefault();

			if( testConfig != null )
				this.Config = new VolusionConfig( testConfig.ShopName, testConfig.UserName, testConfig.Password );
		}

		[ Test ]
		public void GetPublicProducts()
		{
			var service = this.VolusionFactory.CreateProductsService( this.Config );
			var products = service.GetPublicProducts();

			products.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public async Task GetPublicProductsAsync()
		{
			var service = this.VolusionFactory.CreateProductsService( this.Config );
			var products = await service.GetPublicProductsAsync();

			products.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void GetProducts()
		{
			var service = this.VolusionFactory.CreateProductsService( this.Config );
			var products = service.GetProducts();

			products.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public async Task GetProductsAsync()
		{
			var service = this.VolusionFactory.CreateProductsService( this.Config );
			var products = await service.GetProductsAsync();

			products.Count().Should().BeGreaterThan( 0 );
		}

		[ Test ]
		public void ProductQuantityUpdated()
		{
			var service = this.VolusionFactory.CreateProductsService( this.Config );

			var productToUpdate = new VolusionProduct { Sku = "ah-chairbamboo", Quantity = 26 };
			service.UpdateProducts( new List< VolusionProduct > { productToUpdate } );
		}

		[ Test ]
		public async Task ProductQuantityUpdatedAsync()
		{
			var service = this.VolusionFactory.CreateProductsService( this.Config );

			var productToUpdate = new VolusionProduct { Sku = "ah-chairbamboo", Quantity = 55 };
			await service.UpdateProductsAsync( new List< VolusionProduct > { productToUpdate } );
		}
	}
}