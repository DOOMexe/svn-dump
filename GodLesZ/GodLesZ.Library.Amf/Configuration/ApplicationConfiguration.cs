using System.IO;
using System.Xml;
using System.Xml.Serialization;
using GodLesZ.Library.Amf.Messaging;

namespace GodLesZ.Library.Amf.Configuration {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class ConfigurationSection {
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	[XmlRootAttribute("configuration")]
	public class ApplicationConfiguration {
		ApplicationHandlerConfiguration _applicationHandler;
		StreamFilenameGeneratorConfiguration _streamFilenameGenerator;
		SharedObjectServiceConfiguration _sharedObjectServiceConfiguration;
		ProviderServiceConfiguration _providerService;
		ConsumerServiceConfiguration _consumerServiceConfiguration;
		StreamServiceConfiguration _streamService;
		SharedObjectSecurityServiceConfiguration _sharedObjectSecurityService;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("application-handler")]
		public ApplicationHandlerConfiguration ApplicationHandler {
			get {
				if (_applicationHandler == null)
					return _applicationHandler = new ApplicationHandlerConfiguration();
				return _applicationHandler;
			}
			set { _applicationHandler = value; }
		}
		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("streamFilenameGenerator")]
		public StreamFilenameGeneratorConfiguration StreamFilenameGenerator {
			get {
				if (_streamFilenameGenerator == null)
					return _streamFilenameGenerator = new StreamFilenameGeneratorConfiguration();
				return _streamFilenameGenerator;
			}
			set { _streamFilenameGenerator = value; }
		}
		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("sharedObjectService")]
		public SharedObjectServiceConfiguration SharedObjectServiceConfiguration {
			get {
				if (_sharedObjectServiceConfiguration == null)
					return _sharedObjectServiceConfiguration = new SharedObjectServiceConfiguration();
				return _sharedObjectServiceConfiguration;
			}
			set { _sharedObjectServiceConfiguration = value; }
		}
		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("streamService")]
		public StreamServiceConfiguration StreamService {
			get {
				if (_streamService == null)
					return _streamService = new StreamServiceConfiguration();
				return _streamService;
			}
			set { _streamService = value; }
		}
		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("providerService")]
		public ProviderServiceConfiguration ProviderServiceConfiguration {
			get {
				if (_providerService == null)
					return _providerService = new ProviderServiceConfiguration();
				return _providerService;
			}
			set { _providerService = value; }
		}
		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("consumerService")]
		public ConsumerServiceConfiguration ConsumerServiceConfiguration {
			get {
				if (_consumerServiceConfiguration == null)
					return _consumerServiceConfiguration = new ConsumerServiceConfiguration();
				return _consumerServiceConfiguration;
			}
			set { _consumerServiceConfiguration = value; }
		}
		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("sharedObjectSecurityService")]
		public SharedObjectSecurityServiceConfiguration SharedObjectSecurityService {
			get {
				if (_sharedObjectSecurityService == null)
					return _sharedObjectSecurityService = new SharedObjectSecurityServiceConfiguration();
				return _sharedObjectSecurityService;
			}
			set { _sharedObjectSecurityService = value; }
		}
		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static ApplicationConfiguration Load(string path) {
			if (!File.Exists(path))
				return new ApplicationConfiguration();

			using (StreamReader streamReader = new StreamReader(path)) {
				XmlSerializer serializer = new XmlSerializer(typeof(ApplicationConfiguration));
				ApplicationConfiguration config = serializer.Deserialize(streamReader) as ApplicationConfiguration;
				streamReader.Close();
				return config;
			}
		}
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class ApplicationHandlerConfiguration : ConfigurationSection {
		string _type;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				if (_type != null)
					return _type;
				return typeof(GodLesZ.Library.Amf.Messaging.Adapter.ApplicationAdapter).FullName;
			}
			set { _type = value; }
		}

		string _factory;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("factory")]
		public string Factory {
			get {
				if (_factory != null)
					return _factory;
				return DotNetFactory.Id;
			}
			set { _factory = value; }
		}

	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class StreamFilenameGeneratorConfiguration : ConfigurationSection {
		string _type;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				if (_type != null)
					return _type;
				return typeof(GodLesZ.Library.Amf.Messaging.Rtmp.Stream.DefaultStreamFilenameGenerator).FullName;
			}
			set { _type = value; }
		}
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class SharedObjectServiceConfiguration : ConfigurationSection {
		string _type;
		PersistenceStoreConfiguration _persistenceStore;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				if (_type != null)
					return _type;
				return typeof(GodLesZ.Library.Amf.Messaging.Rtmp.SO.SharedObjectService).FullName;
			}
			set { _type = value; }
		}

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlElement("persistenceStore")]
		public PersistenceStoreConfiguration PersistenceStore {
			get {
				if (_persistenceStore == null)
					return _persistenceStore = new PersistenceStoreConfiguration();
				return _persistenceStore;
			}
			set { _persistenceStore = value; }
		}
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class PersistenceStoreConfiguration {
		string _type;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				if (_type != null)
					return _type;
				return typeof(GodLesZ.Library.Amf.Messaging.Rtmp.Persistence.MemoryStore).FullName;
			}
			set { _type = value; }
		}
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class ProviderServiceConfiguration : ConfigurationSection {
		string _type;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				if (_type != null)
					return _type;
				return typeof(GodLesZ.Library.Amf.Messaging.Rtmp.Stream.ProviderService).FullName;
			}
			set { _type = value; }
		}
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class ConsumerServiceConfiguration : ConfigurationSection {
		string _type;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				if (_type != null)
					return _type;
				return typeof(GodLesZ.Library.Amf.Messaging.Rtmp.Stream.ConsumerService).FullName;
			}
			set { _type = value; }
		}
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class StreamServiceConfiguration : ConfigurationSection {
		string _type;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				if (_type != null)
					return _type;
				return typeof(GodLesZ.Library.Amf.Messaging.Rtmp.Stream.StreamService).FullName;
			}
			set { _type = value; }
		}
	}

	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class SharedObjectSecurityServiceConfiguration : ConfigurationSection {
		string _type;

		/// <summary>
		/// This member supports the infrastructure and is not intended to be used directly from your code.
		/// </summary>
		[XmlAttribute("type")]
		public string Type {
			get {
				return _type;
			}
			set { _type = value; }
		}
	}

}
