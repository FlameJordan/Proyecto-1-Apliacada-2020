/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Backend_Aplicada;

import GUI.MainWindow;
import Logica.ConexionPosgres;

/**
 *
 * @author brend
 */
public class Main {
   public static ConexionPosgres con;
    public static void main (String args[]){
        con = new ConexionPosgres();
        con.realizaConexion();
        MainWindow main = new MainWindow();
        main.init();
        
    }
    
}
