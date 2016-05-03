package com.httplistener;

import java.io.IOException;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.eclipse.jetty.http.HttpMethod;

//���� ��� ������� ��������������� ��������� ����� ����������, 
//� �� �� ������ ��������� ��������� serialVersionUID � �������, 
//����� ������ ����� ������� �������� ������ �������� ��������� @SuppressWarnings(�serial�).
@SuppressWarnings("serial")
public class Http_Listener1 extends HttpServlet
{
       

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException
	{
		// TODO Auto-generated method stub
		 request.setCharacterEncoding("UTF-8");
		 String username = (String) request.getParameter("username");
		 username = username.toUpperCase();
		 response.setContentType("text/html;charset=UTF-8");
		 response.getWriter().println("<!DOCTYPE HTML>");
		 response.getWriter().println("<html><body><p>" + "��� ������������: " + username + "</p></body></html>");

	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException 
	{
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
