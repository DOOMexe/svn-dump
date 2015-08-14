using System;
using System.Collections;
using System.Diagnostics;
using System.Web;
using GodLesZ.Library.Amf.Json.Services;
using GodLesZ.Library.Amf.Util;

namespace GodLesZ.Library.Amf.Json.Rpc {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	class DefaultJsonRpcProxyGenerator : IJsonRpcProxyGenerator {
		#region IJsonRpcProxyGenerator Members

		public void WriteProxy(ServiceClass service, IndentedTextWriter writer, HttpRequest request) {
			ValidationUtils.ArgumentNotNull(writer, "writer");

			writer.WriteLine("// This JavaScript was automatically generated by");
			writer.Write("// ");
			writer.WriteLine(GetType().AssemblyQualifiedName);
			writer.Write("// on ");
			DateTime now = DateTime.Now;
			TimeZone timeZone = TimeZone.CurrentTimeZone;
			writer.Write(now.ToLongDateString());
			writer.Write(" at ");
			writer.Write(now.ToLongTimeString());
			writer.Write(" (");
			writer.Write(timeZone.IsDaylightSavingTime(now) ? timeZone.DaylightName : timeZone.StandardName);
			writer.WriteLine(")");
			writer.WriteLine();

			Uri url = request.Url;

			Debug.Assert(service != null);
			Debug.Assert(url != null);
			Debug.Assert(!url.IsFile);
			Debug.Assert(writer != null);

			writer.WriteLine("// Default Proxy");
			writer.WriteLine();

			writer.Write("function ");
			writer.Write(service.Name);
			writer.WriteLine("(url)");
			writer.WriteLine("{");
			writer.Indent++;

			ICollection methods = service.GetMethods();
			string[] methodNames = new string[methods.Count];

			int i = 0;
			foreach (Method method in methods) {
				methodNames[i++] = method.Name;

				writer.Write("this[\"");
				writer.Write(method.Name);
				writer.Write("\"] = function(");

				Parameter[] parameters = method.GetParameters();

				foreach (Parameter parameter in parameters) {
					writer.Write(parameter.Name);
					writer.Write(", ");
				}

				writer.WriteLine("callback)");
				writer.WriteLine("{");
				writer.Indent++;

				writer.Write("return call(\"");
				writer.Write(method.Name);
				writer.Write("\", [");

				foreach (Parameter parameter in parameters) {
					if (parameter.Position > 0)
						writer.Write(',');
					writer.Write(' ');
					writer.Write(parameter.Name);
				}

				writer.WriteLine(" ], callback);");

				writer.Indent--;
				writer.WriteLine("}");
				writer.WriteLine();
			}

			writer.Write("var url = typeof(url) === 'string' ? url : '");
			writer.Write(url);
			writer.WriteLine("';");
			writer.WriteLine(@"var self = this;
    var nextId = 0;
    var credentials;

    this['setCredentials'] = function(userid, password)
    {
        this.credentials = Base64.encode(userid + ':' + password);
    }

    this['clearCredentials'] = function()
    {
        var request = { id : nextId++, method : 'clearCredentials', params : [] };
        this.credentials = null;
        callSync('clearCredentials', request);
    }

    function call(method, params, callback)
    {
        var request = { id : nextId++, method : method, params : params, credentials:self.credentials };
        return callback == null ? 
            callSync(method, request) : callAsync(method, request, callback);
    }

    function callSync(method, request)
    {
        var http = newHTTP();
        http.open('POST', url, false, self.httpUserName, self.httpPassword);
        setupHeaders(http, method);
        http.send(JSON.stringify(request));
        if (http.status != 200)
            throw { message : http.status + ' ' + http.statusText, toString : function() { return message; } };
        var response = JSON.parse(http.responseText);
        if (response.error != null) throw response.error;
        return response.result;
    }

    function callAsync(method, request, callback)
    {
        var http = newHTTP();
        http.open('POST', url, true, self.httpUserName, self.httpPassword);
        setupHeaders(http, method);
        http.onreadystatechange = function() { http_onreadystatechange(http, callback); }
        http.send(JSON.stringify(request));
        return request.id;
    }

    function setupHeaders(http, method)
    {
        http.setRequestHeader('Content-Type', 'text/plain; charset=utf-8');
        http.setRequestHeader('X-JSON-RPC', method);
    }

    function http_onreadystatechange(sender, callback)
    {
        if (sender.readyState == /* complete */ 4)
        {
            var response = sender.status == 200 ? 
                JSON.parse(sender.responseText) : {};
            
            response.xmlHTTP = sender;
                
            callback(response);
        }
    }

    function newHTTP()
    {
        if (typeof(window) != 'undefined' && window.XMLHttpRequest)
            return new XMLHttpRequest(); /* IE7, Safari 1.2, Mozilla 1.0/Firefox, and Netscape 7 */
        else
            return new ActiveXObject('Microsoft.XMLHTTP'); /* WSH and IE 5 to IE 6 */
    }");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();
			writer.Write(service.Name);
			writer.Write(".rpcMethods = ");
			//JsonTextWriter jsonWriter = new JsonTextWriter(writer);
			//jsonWriter.WriteStringArray(methodNames);
			JsonWriter jsonWriter = new JsonWriter(writer);
			jsonWriter.WriteStringArray(methodNames);
			writer.WriteLine(";");
		}
		#endregion
	}
}
