using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;


namespace Http_Listener
{
    public partial class Form1 : Form
    {
        HttpListener server = new HttpListener();
        bool flag = true;
        string name;
        
        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {

            StartServer();
        }

        //Метод StartServer — запускает сервер, принимает входящие запросы,
        //отправленные методами: GET или POST на uri адрес: http://192.168.10.1:8080/say.
        //А также создаёт динамическую страницу, которая содержит форму с текстовым полем и
        //кнопкой типа Submit для отправки клиенту. Для вызова метода Start требуются права
        //администратора(запустить Visual Studio от имени Администратора), иначе возникает ошибка
        //«Отказано в доступе”.
        public async void StartServer()
        {
            //  server = new HttpListener();
            // HttpListener server = new HttpListener();

            // текущая ос не поддерживается
            // if (!HttpListener.IsSupported) return;

            //добавление префикса (say/), т.е. нужного нам сайта
            //обязательно в конце должна быть косая черта
            //  if (string.IsNullOrEmpty(prefix))
            //      throw new ArgumentException("prefix");

            string uri = ("http://localhost:8080/say/");
            // string parametr = ("age = 123");
            // string data = ("age=123");
            // server.Prefixes.Add("http://localhost:8080/say/");
            server.Prefixes.Add(uri);

            //запускаем сервер
            server.Start();

            this.Text = "Сервер запущен!";

            //Сервер запущен? Тогда слушаем входящие соединения
            // while (server.IsListening)
            // {
            //ожидаем входящие запросы
            //HttpListenerContext context = server.GetContext();

            //  IAsyncResult result = server.BeginGetContext(new AsyncCallback(ListenerCallback), server);

            //получаем входящий запрос
            // HttpListenerRequest request = context.Request;             
            // }
            //   result.AsyncWaitHandle.WaitOne();
            //  server.Close();


            // public delegate void AddMessageDelegate(string message);
            //  public delegate void AddMessageDelegate1(string message1);

            // public void LogAdd(string message)
            //  {
            //     textBox1.AppendText(message);
            //  }

            //public void LogAdd1(string message1)
            //{
            //    textBox1.AppendText(message1);
            //  }

            // public void ListenerCallback(IAsyncResult result)
            // {
            //  HttpListener server = (HttpListener)result.AsyncState;
            // Call EndGetContext to complete the asynchronous operation.
            // HttpListenerContext context = server.EndGetContext(result);
            while (true) //чтобы постоянно прослушивать входящие подключения
            {
                HttpListenerContext context = await server.GetContextAsync(); //await для асинхронной обработки
                HttpListenerRequest request = context.Request;
                //обрабатываем POST запрос
                //запрос получен методом POST (пришли данные формы)
                if (request.HttpMethod == "POST")
                {
                    textBox1.Text += Environment.NewLine + ("Был использован POST запрос"); //Environment.NewLine - чтобы выводить с новой строки
                    // Invoke(new AddMessageDelegate(LogAdd), new object[] { "Был использован POST запрос" }); //проблемы с потоком, доступ из одного потока в другой

                    //показать, что пришло от клиента
                    ShowRequestData(request);

                    //завершаем работу сервера
                    if (!flag) return;
                }

                  if (request.HttpMethod == "GET")
                 {
                   // if (HttpListenerRequest  req = HttpListenerRequest
                //     Invoke(new AddMessageDelegate(LogAdd), new object[] { "Был использован GET запрос" });
                     textBox1.Text = ("Был использован GET запрос");

                    //if (request.QueryString != null)
                    // { textBox1.Text += Environment.NewLine + ("Параметры: {0}") + request.QueryString.ToString(); }
                    // { textBox1.Text += Environment.NewLine + ("Параметры: {0}") + httputility.ParseQueryString; }



                     string qs = "?age=123&name=Ann";

                    //Uri strUri = new Uri(qs);
                    // String Query = strUri.Query;
                    //string sName = HttpUtility.ParseQueryString(Query).Get("name");
                    //NameValueCollection query = HttpUtility.ParseQueryString(qs);
                    textBox1.Text += Environment.NewLine + ("Параметры:") + HttpUtility.ParseQueryString(qs).Get("name") +
                            HttpUtility.ParseQueryString(qs).Get("age");
                    

                }


                //формируем ответ сервера:
                //динамически создаём страницу
                string htmlpage = @"<!DOCTYPE HTML>
                    <html><head></head><body>
                    <form method=""post"" action=""say"">
                    <p><b>Name: {0}</b><br>
                    <input type=""text"" name=""myname"" size=""40""></p>
                    <p><input type=""submit"" value=""send""></p>
                    </form></body></html>";

                string responseString = string.Format(htmlpage, name);


                // string responseString = string.Format(htmlpage, lname);
                //отправка данных клиенту
                HttpListenerResponse response = context.Response;
                response.ContentType = "text/html; charset=UTF-8";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;

               // int age = 10;
               // response.Redirect("http://localhost:8080/say/?age=" + age.ToString());

                using (Stream output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }
        }

      //  }

        //Метод ShowRequestData — обрабатывает http запросы, отправленные методом POST,
        //и выводит введённое имя. Если клиент отправляет слово stop, то веб-сервер приостанавливает 
        //свою работу. Если вместо метода Stop вызвать метод Close, то сервер полностью завершит свою работу.
        private void ShowRequestData (HttpListenerRequest request)
        {
            //есть данные от клиента?
            if (!request.HasEntityBody) return;

            //смотрим, что пришло
            using (Stream body = request.InputStream)
            {

                using (StreamReader reader = new StreamReader(body))
                {
                    string text = reader.ReadToEnd();                 

                    //оставляем только имя
                    text = text.Remove(0,7);

                    //преобразуем %CC%E0%EA%F1 -> Аня
                    text = System.Web.HttpUtility.UrlDecode(text, Encoding.UTF8);

                    //выводим имя
                    // Invoke(new AddMessageDelegate(LogAdd), new object[] { "Ваше имя: " + text });
                     textBox1.Text += Environment.NewLine + ("Ваше имя: " + text);
                    //MessageBox.Show("Ваше имя: " + text);
                    name = text;

                    flag = true;

                    //останавливаем сервер
                    if (text == "stop")
                    {
                        server.Stop();
                        this.Text = "Сервер остановлен!";
                        flag = false;
                    }
                }
            }
        }

    }
}
