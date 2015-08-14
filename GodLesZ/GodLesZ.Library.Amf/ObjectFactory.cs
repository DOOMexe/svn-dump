
using System;
using System.Reflection;
using System.Security.Permissions;
using GodLesZ.Library.Amf.Collections.Generic;
using GodLesZ.Library.Amf.Configuration;
#if !SILVERLIGHT
using GodLesZ.Library.Amf.Reflection;
using GodLesZ.Library.Amf.Reflection.Lightweight;
using GodLesZ.Library.Amf.Util;
using log4net;
#endif

namespace GodLesZ.Library.Amf {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	sealed class ObjectFactory {
#if !SILVERLIGHT
		private static readonly ILog Log = LogManager.GetLogger(typeof(ObjectFactory));
#endif
		private static volatile ObjectFactory _instance;
		private static readonly object SyncRoot = new Object();

		private readonly CopyOnWriteDictionary<string, Type> _typeCache;
#if !SILVERLIGHT
		private readonly CopyOnWriteDictionary<Type, ConstructorInvoker> _typeConstructorCache;
#endif
		private readonly string[] _lacLocations;
		private readonly bool _reflectionEmitPermission;

		private ObjectFactory() {
			_lacLocations = TypeHelper.GetLacLocations();
			_typeCache = new CopyOnWriteDictionary<string, Type>();
#if !SILVERLIGHT
			_typeConstructorCache = new CopyOnWriteDictionary<Type, ConstructorInvoker>();
			try {
				new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit).Demand();
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
				new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				_reflectionEmitPermission = true;
			} catch (Exception ex) {
				Unreferenced.Parameter(ex);
				_reflectionEmitPermission = false;
			}
#endif
		}

		public static ObjectFactory Instance {
			get {
				if (_instance == null) {
					lock (SyncRoot) {
						if (_instance == null)
							_instance = new ObjectFactory();
					}
				}
				return _instance;
			}
		}

		public Type InternalLocate(string typeName) {
			if (string.IsNullOrEmpty(typeName))
				return null;

			string mappedTypeName = FluorineConfiguration.Instance.GetMappedTypeName(typeName);
			//Lookup first in our cache.
			Type type;
			if (!_typeCache.TryGetValue(mappedTypeName, out type)) {
				type = TypeHelper.Locate(mappedTypeName);
				if (type != null)
					_typeCache[mappedTypeName] = type;
				else
					type = InternalLocateInLac(mappedTypeName); // Locate in the LAC
			}
			return type;
		}

		public Type InternalLocateInLac(string typeName) {
			if (string.IsNullOrEmpty(typeName))
				return null;

			string mappedTypeName = FluorineConfiguration.Instance.GetMappedTypeName(typeName);
			//Lookup first in our cache.
			Type type;
			if (!_typeCache.TryGetValue(mappedTypeName, out type)) {
				//Locate in LAC
				for (int i = 0; i < _lacLocations.Length; i++) {
					type = TypeHelper.LocateInLac(mappedTypeName, _lacLocations[i]);
					if (type != null) {
						_typeCache[mappedTypeName] = type;
						return type;
					}
				}
			}
			return type;
		}

		internal void AddTypeToCache(Type type) {
			if (type != null)
				_typeCache[type.FullName] = type;
		}

		public bool ContainsType(string typeName) {
			if (string.IsNullOrEmpty(typeName))
				return false;
			return _typeCache.ContainsKey(typeName);
		}

		public object InternalCreateInstance(Type type) {
			return InternalCreateInstance(type, null);
		}

		public object InternalCreateInstance(string typeName) {
			return InternalCreateInstance(typeName, null);
		}

		public object InternalCreateInstance(string typeName, object[] args) {
			Type type = InternalLocate(typeName);
			return InternalCreateInstance(type, args);
		}

		public object InternalCreateInstance(Type type, object[] args) {
			if (type != null) {
				if (type.IsAbstract && type.IsSealed)
					return type;

#if !SILVERLIGHT
				if (_reflectionEmitPermission) {
					ConstructorInvoker invoker;
					_typeConstructorCache.TryGetValue(type, out invoker);
					if (invoker == null) {
						invoker = ConstructorExtensions.DelegateForCreateInstance(type, args);
						_typeConstructorCache[type] = invoker;
					}
					return invoker(args);
				}
				return Activator.CreateInstance(type, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null, args, null);
#else

				return type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Static, null, null, args);
#endif
			}
			return null;
		}

		static public Type Locate(string type) {
			return Instance.InternalLocate(type);
		}

		static public Type LocateInLac(string type) {
			return Instance.InternalLocateInLac(type);
		}

		static public object CreateInstance(Type type) {
			return Instance.InternalCreateInstance(type);
		}

		static public object CreateInstance(string type) {
			return Instance.InternalCreateInstance(type);
		}

		static public object CreateInstance(Type type, object[] args) {
			return Instance.InternalCreateInstance(type, args);
		}
	}
}
