/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objeto;

/**
 *
 * @author brend
 */
public class Producto {

    private int idProducto;
    private int idEmpresa;
    private String nombre;
    private int precio;
    private String descripcion;
    private int cantStock;
    private int tipo;
    private int estado;

    public Producto() {
        this.idProducto = 0;
        this.idEmpresa = 0;
        this.nombre = "";
        this.precio = 0;
        this.cantStock = 0;
        this.descripcion = "";
        this.cantStock = 0;
        this.tipo=0;
        this.estado=0;
    }

    public int getIdProducto() {
        return idProducto;
    }

    public int getIdEmpresa() {
        return idEmpresa;
    }

    public String getNombre() {
        return nombre;
    }

    public int getPrecio() {
        return precio;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public int getCantStock() {
        return cantStock;
    }

    public int getEstado() {
        return estado;
    }

    public int getTipo() {
        return tipo;
    }
    

    public void setIdProducto(int idProducto) {
        this.idProducto = idProducto;
    }

    public void setIdEmpresa(int idEmpresa) {
        this.idEmpresa = idEmpresa;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public void setPrecio(int precio) {
        this.precio = precio;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public void setCantStock(int cantStock) {
        this.cantStock = cantStock;
    }

    public void setEstado(int estado) {
        this.estado = estado;
    }

    public void setTipo(int tipo) {
        this.tipo = tipo;
    }
    
    
}
