using System;
using System.Collections;
using GodLesZ.Library.Amf.Configuration;
using GodLesZ.Library.Amf.Messaging.Api;
using GodLesZ.Library.Amf.Messaging.Api.Persistence;
using GodLesZ.Library.Amf.Messaging.Api.SO;
using GodLesZ.Library.Amf.Messaging.Rtmp.Persistence;
// Import log4net classes.
using log4net;


namespace GodLesZ.Library.Amf.Messaging.Rtmp.SO {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	class SharedObjectService : ISharedObjectService {
		ILog _log;

		public static string ScopeType = "SharedObject";
		private static string SO_PERSISTENCE_STORE = Constants.TransientPrefix + "_SO_PERSISTENCE_STORE_";
		private static string SO_TRANSIENT_STORE = Constants.TransientPrefix + "_SO_TRANSIENT_STORE_";

		SharedObjectServiceConfiguration _configuration;

		public SharedObjectService() {
			try {
				_log = LogManager.GetLogger(typeof(SharedObjectService));
			} catch { }
		}

		#region ISharedObjectService Members

		public void Start(ConfigurationSection configuration) {
			_configuration = configuration as SharedObjectServiceConfiguration;
		}

		public void Shutdown() {
		}

		public ICollection GetSharedObjectNames(IScope scope) {
			ArrayList result = new ArrayList();
			IEnumerator enumerator = scope.GetBasicScopeNames(ScopeType);
			while (enumerator.MoveNext()) {
				result.Add(enumerator.Current);
			}
			return result;
		}

		public bool CreateSharedObject(IScope scope, string name, bool persistent) {
			if (HasSharedObject(scope, name)) {
				// The shared object already exists.
				return true;
			}
			IBasicScope soScope = new SharedObjectScope(scope, name, persistent, GetStore(scope, persistent));
			return scope.AddChildScope(soScope);
		}

		public ISharedObject GetSharedObject(IScope scope, string name) {
			return scope.GetBasicScope(ScopeType, name) as ISharedObject;
		}

		public ISharedObject GetSharedObject(IScope scope, string name, bool persistent) {
			if (!HasSharedObject(scope, name)) {
				CreateSharedObject(scope, name, persistent);
			}
			return GetSharedObject(scope, name);
		}

		public bool HasSharedObject(IScope scope, string name) {
			return scope.HasChildScope(ScopeType, name);
		}

		public bool ClearSharedObjects(IScope scope, string name) {
			bool result = false;
			if (HasSharedObject(scope, name)) {
				// "/" clears all local and persistent shared objects associated
				// with the instance
				// if (name.equals("/")) {
				// /foo/bar clears the shared object /foo/bar; if bar is a directory
				// name, no shared objects are deleted.
				// if (name.equals("/")) {
				// /foo/bar/* clears all shared objects stored under the instance
				// directory /foo/bar. The bar directory is also deleted if no
				// persistent shared objects are in use within this namespace.
				// if (name.equals("/")) {
				// /foo/bar/XX?? clears all shared objects that begin with XX,
				// followed by any two characters. If a directory name matches this
				// specification, all the shared objects within this directory are
				// cleared.
				// if (name.equals("/")) {
				// }
				result = (scope.GetBasicScope(ScopeType, name) as ISharedObject).Clear();
			}
			return result;
		}

		#endregion

		private IPersistenceStore GetStore(IScope scope, bool persistent) {
			IPersistenceStore store = null;
			if (!persistent) {
				// Use special store for non-persistent shared objects
				if (!scope.HasAttribute(SO_TRANSIENT_STORE)) {
					store = new MemoryStore(scope);
					scope.SetAttribute(SO_TRANSIENT_STORE, store);
					return store;
				}
				return scope.GetAttribute(SO_TRANSIENT_STORE) as IPersistenceStore;
			}
			// Evaluate configuration for persistent shared objects
			if (!scope.HasAttribute(SO_PERSISTENCE_STORE)) {
				try {
					Type type = ObjectFactory.Locate(_configuration.PersistenceStore.Type);
					store = Activator.CreateInstance(type, new object[] { scope }) as IPersistenceStore;
					if (_log.IsInfoEnabled)
						_log.Info(__Res.GetString(__Res.SharedObjectService_CreateStore, store));
				} catch (Exception exception) {
					if (_log.IsErrorEnabled)
						_log.Error(__Res.GetString(__Res.SharedObjectService_CreateStoreError), exception);
					store = new MemoryStore(scope);
				}
				scope.SetAttribute(SO_PERSISTENCE_STORE, store);
				return store;
			}
			return scope.GetAttribute(SO_PERSISTENCE_STORE) as IPersistenceStore;
		}

	}
}