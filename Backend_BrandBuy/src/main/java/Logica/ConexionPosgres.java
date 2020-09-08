/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Logica;

import Objeto.Producto;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;

/**
 *
 * @author brend
 */
public class ConexionPosgres {

    public Connection conSQL = null;
    public ArrayList<Producto> listaProducto;

    public void realizaConexion() {
      listaProducto = new ArrayList<>();
        String urlDatabase = "jdbc:postgresql://163.178.107.7:5432/IF6201_Proyecto#1_Brandbuy";
        try {
            Class.forName("org.postgresql.Driver");
            conSQL = DriverManager.getConnection(urlDatabase, "laboratorios", "saucr.120");
            System.out.println("La conexión se realizo sin problemas! =) ");
        } catch (Exception e) {
            System.out.println("Ocurrio un error : " + e.getMessage());
        }
    }

//    public void insertar() {
//        Statement stmt = null;
//        try {
//            
//            
//            conn.setAutoCommit(false);
//            System.out.println("Opened database successfully");
//
//            stmt = conn.createStatement();
//            String sql = "INSERT INTO ESTUDIANTES (NOMBRE,EDAD) "
//                    + "VALUES ('Graci porras',40 );";
//            stmt.executeUpdate(sql);
//
//            stmt.close();
//            conn.commit();
//            conn.close();
//        } catch (Exception e) {
//            System.err.println(e.getClass().getName() + ": " + e.getMessage());
//            System.exit(0);
//        }
//        System.out.println("Records created successfully");
//    }
    
    public void select(){
        
      Statement stmt = null;
      try {
        
         conSQL.setAutoCommit(false);
         System.out.println("Opened database successfully");
         stmt = conSQL.createStatement();
         ResultSet rs = stmt.executeQuery( "select * FROM obtenerProductos()" );
         while ( rs.next() ) {
             Producto p1 = new Producto();
             
            p1.setIdProducto(rs.getInt("idproductoT"));
            p1.setIdEmpresa(rs.getInt("idempresaT"));
            p1.setNombre(rs.getString("nombreT"));
            p1.setPrecio(rs.getInt("precioT"));
            p1.setDescripcion(rs.getString("descripcionT"));
            p1.setCantStock(rs.getInt("cantstockT"));
            p1.setTipo(rs.getInt("tipoT"));
            p1.setEstado(rs.getInt("estadoT"));
           
             listaProducto.add(p1);
             
         }
         rs.close();
         stmt.close();
      } catch ( Exception e ) {
         System.err.println( e.getClass().getName()+": "+ e.getMessage() );
         System.exit(0);
      }
      System.out.println("Operation done successfully");
      
   }
    
}
