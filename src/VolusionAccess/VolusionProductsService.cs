﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using VolusionAccess.Misc;
using VolusionAccess.Models.Configuration;
using VolusionAccess.Models.Product;
using VolusionAccess.Services;

namespace VolusionAccess
{
	public class VolusionProductsService : VolusionServiceBase, IVolusionProductsService
	{
		private readonly WebRequestServices _webRequestServices;
		private readonly VolusionConfig _config;

		public VolusionProductsService( VolusionConfig config )
		{
			Condition.Requires( config, "config" ).IsNotNull();
			this._webRequestServices = new WebRequestServices( config );
			_config = config;
		}

		#region Get
		public IEnumerable< VolusionProduct > GetPublicProducts()
		{
			var products = new List< VolusionProduct >();
			var endpoint = EndpointsBuilder.CreateGetPublicProductsEndpoint().GetFullEndpoint( _config );

			ActionPolicies.Get.Do( () =>
			{
				var tmp = this._webRequestServices.GetResponseForSpecificUrl< VolusionPublicProducts >( endpoint );
				if( tmp != null && tmp.Products != null && tmp.Products.Count > 0 )
					products.AddRange( tmp.Products );

				//API requirement
				this.CreateApiDelay().Wait();
			} );

			return products;
		}

		public async Task< IEnumerable< VolusionProduct > > GetPublicProductsAsync()
		{
			var products = new List< VolusionProduct >();
			var endpoint = EndpointsBuilder.CreateGetPublicProductsEndpoint().GetFullEndpoint( _config );

			await ActionPolicies.GetAsync.Do( async () =>
			{
				var tmp = await this._webRequestServices.GetResponseForSpecificUrlAsync< VolusionPublicProducts >( endpoint );
				if( tmp != null && tmp.Products != null && tmp.Products.Count > 0 )
					products.AddRange( tmp.Products );

				//API requirement
				this.CreateApiDelay().Wait();
			} );

			return products;
		}

		public IEnumerable< VolusionProduct > GetProducts()
		{
			var products = new List< VolusionProduct >();
			IList< VolusionProduct > productsPortion = null;
			var endpoint = EndpointsBuilder.CreateGetProductsEndpoint();

			do
			{
				ActionPolicies.Get.Do( () =>
				{
					var tmp = this._webRequestServices.GetResponse< VolusionProducts >( endpoint );
					productsPortion = tmp != null ? tmp.Products : null;
					if( productsPortion != null )
						products.AddRange( productsPortion );

					//API requirement
					this.CreateApiDelay().Wait();
				} );
			} while( productsPortion != null && productsPortion.Count != 0 );

			return products;
		}

		public async Task< IEnumerable< VolusionProduct > > GetProductsAsync()
		{
			var products = new List< VolusionProduct >();
			IList< VolusionProduct > productsPortion = null;
			var endpoint = EndpointsBuilder.CreateGetProductsEndpoint();

			do
			{
				await ActionPolicies.GetAsync.Do( async () =>
				{
					var tmp = await this._webRequestServices.GetResponseAsync< VolusionProducts >( endpoint );
					productsPortion = tmp != null ? tmp.Products : null;
					if( productsPortion != null )
						products.AddRange( productsPortion );

					//API requirement
					this.CreateApiDelay().Wait();
				} );
			} while( productsPortion != null && productsPortion.Count != 0 );

			return products;
		}
		#endregion

		#region Update
		/// <summary>
		/// Update products
		/// </summary>
		/// <param name="products">The products. Need to use SKU as key.</param>
		public void UpdateProducts( IEnumerable< VolusionProduct > products )
		{
			var endpoint = EndpointsBuilder.CreateProductsUpdateEndpoint();
			var vp = new VolusionProducts { Products = products.ToList() };
			var xmlContent = XmlSerializeHelpers.Serialize( vp );

			ActionPolicies.Submit.Do( () =>
			{
				this._webRequestServices.PostData( endpoint, xmlContent );

				//API requirement
				this.CreateApiDelay().Wait();
			} );
		}

		/// <summary>
		/// Update products
		/// </summary>
		/// <param name="products">The products. Need to use SKU as key.</param>
		public async Task UpdateProductsAsync( IEnumerable< VolusionProduct > products )
		{
			var endpoint = EndpointsBuilder.CreateProductsUpdateEndpoint();
			var vp = new VolusionProducts { Products = products.ToList() };
			var xmlContent = XmlSerializeHelpers.Serialize( vp );

			await ActionPolicies.SubmitAsync.Do( async () =>
			{
				await this._webRequestServices.PostDataAsync( endpoint, xmlContent );

				//API requirement
				this.CreateApiDelay().Wait();
			} );
		}
		#endregion
	}
}