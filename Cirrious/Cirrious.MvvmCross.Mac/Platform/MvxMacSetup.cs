#region Copyright
// <copyright file="MvxBaseTouchSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public abstract class MvxMacSetup
        : MvxSetup
        , IMvxServiceProducer
    {
        private readonly MvxApplicationDelegate _applicationDelegate;
        private readonly IMvxMacViewPresenter _presenter;

        protected MvxMacSetup(MvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
			_presenter = presenter;
			_applicationDelegate = applicationDelegate;
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
			var toReturn = new MvxLoaderBasedPluginManager();
			var registry = new MvxLoaderPluginRegistry(".Mac", toReturn.Loaders);
			AddPluginsLoaders(registry);
			return toReturn;
		}
		
		protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
		{
			// none added by default
		}

        protected sealed override MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxMacViewsContainer();
            RegisterTouchViewCreator(container);            
            return container;
        }

        protected void RegisterTouchViewCreator(MvxMacViewsContainer container)
        {
            this.RegisterServiceInstance<IMvxMacViewCreator>(container);
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxMacViewDispatcherProvider(_presenter);
        }
	
		protected override void InitializePlatformServices ()
		{
            this.RegisterServiceInstance<IMvxLifetime>(_applicationDelegate);
		}

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof(IMvxMacView));
        }
    }
}