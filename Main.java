import java.net. * ;
import java.io. * ;
import java.util. * ;

public class Main {
  int port;
  List < Socket > clients;
  ServerSocket server;
  public static void main(String[] args) {
    new Main();
  }
  public Main() {
    try {
      port = 8899; //端口号
      clients = new ArrayList < Socket > (); //实例化socket list对象
      server = new ServerSocket(port); //开启服务器socket
      while (true) {
        Socket socket = server.accept(); //死循环监听客户端连接
        clients.add(socket); //向保存客户端socket list添加socket实例
        Mythread mythread = new Mythread(socket, clients.size() - 1); //实例化新线程并传送当前socket实例id
        mythread.start(); //开启一个新线程
      }

    } catch(Exception ex) {}
  }
  class Mythread extends Thread { //子线程负责每个客户端消息推送
    Socket tsocket;
    private BufferedReader br;
    private PrintWriter pw;
    public String msg;
    public int id;
    public Mythread(Socket s, int is) {
      tsocket = s;
      id = id;
    }
    public void run() {

      try {
        br = new BufferedReader(new InputStreamReader(tsocket.getInputStream(), "UTF-8"));
        msg = "【IP:" + tsocket.getInetAddress() + "已上线】 在线人数:" + clients.size();

        sendMsg();//上线推送

        while ((msg = br.readLine()) != null) {//循环接收客户端消息

          if (msg.equals("exitthis")) { //如果收到exitthis则退出线程，回收socket。
            clients.remove(id);
            break;

          }
          sendMsg();//推送消息到所有客户端

        }
      } catch(Exception ex) {

}
    }
    //发送信息
    public void sendMsg() {
      try {

        System.out.println(msg);

        for (int i = clients.size() - 1; i >= 0; i--) { //循环推送
          pw = new PrintWriter(clients.get(i).getOutputStream(), true);
          pw.println(msg);
          pw.flush();
        }

      } catch(Exception ex) {}
    }
  }

}