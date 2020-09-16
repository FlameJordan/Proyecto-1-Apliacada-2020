/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package GUI;

import Backend_Aplicada.Main;
import Objeto.Producto;
import java.awt.Color;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.mail.Message;
import javax.mail.PasswordAuthentication;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MailDateFormat;
import javax.mail.internet.MimeMessage;
import javax.swing.BoxLayout;
import javax.swing.ComboBoxModel;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;
import javax.swing.table.TableRowSorter;

/**
 *
 * @author brend
 */
public class MainWindow extends JFrame implements ActionListener {

    private JButton jbtnPrueba;
    private JTable jTableG;
    private JComboBox jComboEmpresas;
    private JComboBox jComboTipos;
    int filaCheck;
    private ArrayList idEmpresasList;
    panelTablaProductos panel;
    private JLabel jlEmpresas;
    private JLabel jlTipo;
    private JButton jbDeshabilitar;
    private JButton jbActivar;
    private Session session;
    private final String username = "brandbuy777@gmail.com";
    private final String password = "veguetita777";

    public void init() {

        Properties props = new Properties();
        props.put("mail.smtp.auth", "true");
        props.put("mail.smtp.starttls.enable", "true");
        props.put("mail.smtp.host", "smtp.gmail.com");
        props.put("mail.smtp.port", "587");
        session = Session.getInstance(props,
                new javax.mail.Authenticator() {
            @Override
            protected PasswordAuthentication getPasswordAuthentication() {
                return new PasswordAuthentication(username, password);
            }
        });

        this.filaCheck = 0;
        this.idEmpresasList = new ArrayList();
        this.setLayout(null);
        this.setBounds(0, 0, 800, 600);
        this.setDefaultCloseOperation(EXIT_ON_CLOSE);
        this.panel = new panelTablaProductos();
        this.jlEmpresas = new JLabel("Selección de Empresa");
        this.jlEmpresas.setBounds(460, 20, 130, 20);
        this.jlTipo = new JLabel("Tipo de Producto");
        this.jbActivar = new JButton("Activar");
        this.jlTipo.setBounds(600, 20, 140, 20);
        this.jComboTipos = new JComboBox();
        this.jComboTipos.addItem("Todos");
        this.jComboTipos.addItem("Arma");
        this.jComboTipos.addItem("Ropa");
        this.jComboTipos.addItem("Tecnologia");
        this.jComboTipos.addItem("Alimento");
        this.jComboTipos.addItem("Farmaco");
        this.jComboTipos.addItem("Servicio");
        this.jComboTipos.addItem("Libreria");
        this.jComboTipos.addItem("Juguetes");
        this.jComboTipos.addItem("Higiene");
        this.jComboTipos.addItem("Cocina");
        this.jComboTipos.addItem("Decoracion");
        this.jComboTipos.setBounds(600, 40, 130, 20);
        this.jbActivar.setBounds(460, 80, 130, 30);
        this.jbDeshabilitar = new JButton("Desactivar");
        this.jbDeshabilitar.setBounds(600, 80, 130, 30);
        this.jbDeshabilitar.addActionListener(this);
        this.jbActivar.addActionListener(this);
        this.add(this.jbDeshabilitar);
        this.add(this.jbActivar);
        this.add(jComboTipos);
        this.setTitle("Backend Brandbuy");
        this.add(jlEmpresas);
        this.add(jlTipo);
        this.add(panel);
        for (int i = 0; i < idEmpresasList.size(); i++) {
            jComboEmpresas.addItem(idEmpresasList.get(i));
        }
        jComboEmpresas.setBounds(460, 40, 130, 20);
        jComboEmpresas.addActionListener(this);
        jComboTipos.addActionListener(this);
        this.add(jComboEmpresas);
        this.setVisible(true);
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        if (jComboEmpresas == e.getSource()) {
            if (!jComboEmpresas.getSelectedItem().equals("Todas")) {
                FiltrarTablaEmpresa(Integer.parseInt(jComboEmpresas.getSelectedItem().toString()), String.valueOf(jComboTipos.getSelectedItem()).toString());
            } else {
                actualizarTabla(String.valueOf(jComboTipos.getSelectedItem()).toString());
            }
        }

        if (jComboTipos == e.getSource()) {
            if (!jComboEmpresas.getSelectedItem().equals("Todas")) {
                FiltrarTablaEmpresa(Integer.parseInt(jComboEmpresas.getSelectedItem().toString()), String.valueOf(jComboTipos.getSelectedItem()).toString());
            } else {
                actualizarTabla(String.valueOf(jComboTipos.getSelectedItem()).toString());
            }
        }

        if (jbDeshabilitar == e.getSource()) {
            try {
                Statement stmt = null;
                Main.con.conSQL.setAutoCommit(false);
                stmt = Main.con.conSQL.createStatement();
                int filas = panel.jTable.getRowCount();
                System.out.println("Fila: " + filas);
                for (int i = 0; i < filas; i++) {
                    Producto p = new Producto();
                    p.setIdProducto(Integer.parseInt((String) panel.jTable.getValueAt(i, 0)));
                    p.setIdEmpresa(Integer.parseInt((String) panel.jTable.getValueAt(i, 1)));

                    if (panel.jTable.getValueAt(i, 7).equals(true)) {
                        try {

                            stmt.executeQuery("select cambiarEstadoProducto" + "(" + p.getIdProducto() + "," + p.getIdEmpresa() + ")");
                            Main.con.conSQL.commit();

                        } catch (SQLException ex) {
                            Logger.getLogger(MainWindow.class.getName()).log(Level.SEVERE, null, ex);
                        }
                    }

                }

                ArrayList<Integer> listEmpresas = new ArrayList<>();
                ArrayList<Producto> listProducto = new ArrayList<>();

                for (int j = 0; j < filas; j++) {
                    if (!listEmpresas.contains(Integer.parseInt((String) panel.jTable.getValueAt(j, 1))) && panel.jTable.getValueAt(j, 7).equals(true)) {
                        listEmpresas.add(Integer.parseInt((String) panel.jTable.getValueAt(j, 1)));
                    }
                }

                if (!listEmpresas.isEmpty()) {
                    for (int j = 0; j < listEmpresas.size(); j++) {
                        for (int k = 0; k < Main.con.listaProducto.size(); k++) {
                            if (Main.con.listaProducto.get(k).getIdEmpresa() == listEmpresas.get(j)) {
                                listProducto.add(Main.con.listaProducto.get(k));
                            }
                        }
                        ResultSet rs = stmt.executeQuery("select correos from obtenerCorreo" + "(" + listEmpresas.get(j) + ")");
                        rs.next();
                        String correo = rs.getString("correos");
                        notificacionVarios(correo, listProducto);
                        listProducto.clear();
                    }
                }

                if (!jComboEmpresas.getSelectedItem().equals("Todas")) {
                    FiltrarTablaEmpresa(Integer.parseInt(jComboEmpresas.getSelectedItem().toString()), String.valueOf(jComboTipos.getSelectedItem()).toString());
                } else {
                    actualizarTabla(String.valueOf(jComboTipos.getSelectedItem()).toString());
                }
                listEmpresas.clear();
                stmt.close();
            } catch (SQLException ex) {
                Logger.getLogger(MainWindow.class.getName()).log(Level.SEVERE, null, ex);
            }

        }

        if (jbActivar == e.getSource()) {
            int filas = panel.jTable.getRowCount();
            System.out.println("Fila: " + filas);
            for (int i = 0; i < filas; i++) {
                Producto p = new Producto();
                p.setIdProducto(Integer.parseInt((String) panel.jTable.getValueAt(i, 0)));
                p.setIdEmpresa(Integer.parseInt((String) panel.jTable.getValueAt(i, 1)));

                if (panel.jTable.getValueAt(i, 7).equals(false)) {
                    try {
                        Statement stmt = null;
                        Main.con.conSQL.setAutoCommit(false);
                        stmt = Main.con.conSQL.createStatement();
                        stmt.executeQuery("select cambiarEstadoProducto" + "(" + p.getIdProducto() + "," + p.getIdEmpresa() + ")");
                        Main.con.conSQL.commit();
                        stmt.close();
                    } catch (SQLException ex) {
                        Logger.getLogger(MainWindow.class.getName()).log(Level.SEVERE, null, ex);
                    }
                }

            }
            if (!jComboEmpresas.getSelectedItem().equals("Todas")) {
                FiltrarTablaEmpresa(Integer.parseInt(jComboEmpresas.getSelectedItem().toString()), String.valueOf(jComboTipos.getSelectedItem()).toString());
            } else {
                actualizarTabla(String.valueOf(jComboTipos.getSelectedItem()).toString());
            }
        }
    }

    public void FiltrarTablaEmpresa(int empresa, String tipo) {
        Main.con.listaProducto.clear();
        Main.con.select();
        DefaultTableModel tableModel = (DefaultTableModel) panel.jTable.getModel();
        int filas = tableModel.getRowCount();
        for (int i = 0; i < filas; i++) {
            tableModel.removeRow(0);
        }
        String[] data = new String[7];
        int contador = 0;
        int tipoInt = 0;

        switch (tipo) {
            case "Todos":
                tipoInt = 0;
                break;
            case "Arma":
                tipoInt = 1;
                break;
            case "Ropa":
                tipoInt = 2;
                break;
            case "Tecnologia":
                tipoInt = 3;
                break;
            case "Alimento":
                tipoInt = 4;
                break;
            case "Farmaco":
                tipoInt = 5;
                break;
            case "Servicio":
                tipoInt = 6;
                break;
            default:
                tipoInt = 0;
                break;
        }

        for (int i = 0; i < Main.con.listaProducto.size(); i++) {
            if (Main.con.listaProducto.get(i).getIdEmpresa() == empresa) {
                if (tipoInt == 0 || tipoInt == Main.con.listaProducto.get(i).getTipo()) {
                    data[0] = String.valueOf(Main.con.listaProducto.get(i).getIdProducto());
                    data[1] = String.valueOf(Main.con.listaProducto.get(i).getIdEmpresa());
                    data[2] = String.valueOf(Main.con.listaProducto.get(i).getNombre());
                    data[3] = String.valueOf(Main.con.listaProducto.get(i).getPrecio());
                    data[4] = String.valueOf(Main.con.listaProducto.get(i).getDescripcion());
                    data[5] = String.valueOf(Main.con.listaProducto.get(i).getCantStock());
                    data[6] = String.valueOf(Main.con.listaProducto.get(i).getTipo());

                    tableModel.addRow(data);
                    if (Main.con.listaProducto.get(i).getEstado() == 1) {
                        tableModel.setValueAt(true, contador, 7);
                    } else {
                        tableModel.setValueAt(false, contador, 7);
                    }
                    contador++;
                }
            }
        }

        tableModel.fireTableDataChanged();
        panel.jTable.setModel(tableModel);
    }

    public void actualizarTabla(String tipo) {
        Main.con.listaProducto.clear();
        Main.con.select();
        DefaultTableModel tableModel = (DefaultTableModel) panel.jTable.getModel();
        int filas = tableModel.getRowCount();
        for (int i = 0; i < filas; i++) {
            tableModel.removeRow(0);
        }
        String[] data = new String[7];
        int tipoInt = 0;
        int contador = 0;
        switch (tipo) {
            case "Todos":
                tipoInt = 0;
                break;
            case "Arma":
                tipoInt = 1;
                break;
            case "Ropa":
                tipoInt = 2;
                break;
            case "Tecnologia":
                tipoInt = 3;
                break;
            case "Alimento":
                tipoInt = 4;
                break;
            case "Farmaco":
                tipoInt = 5;
                break;
            case "Servicio":
                tipoInt = 6;
                break;
            case "Libreria":
                tipoInt = 7;
                break;
            case "Juguetes":
                tipoInt = 8;
                break;
            case "Higiene":
                tipoInt = 9;
                break;
            case "Cocina":
                tipoInt = 10;
                break;
            case "Decoracion":
                tipoInt = 11;
                break;
            default:
                tipoInt = 0;
                break;
        }

        for (int i = 0; i < Main.con.listaProducto.size(); i++) {
            if (tipoInt == 0 || tipoInt == Main.con.listaProducto.get(i).getTipo()) {
                data[0] = String.valueOf(Main.con.listaProducto.get(i).getIdProducto());
                data[1] = String.valueOf(Main.con.listaProducto.get(i).getIdEmpresa());
                data[2] = String.valueOf(Main.con.listaProducto.get(i).getNombre());
                data[3] = String.valueOf(Main.con.listaProducto.get(i).getPrecio());
                data[4] = String.valueOf(Main.con.listaProducto.get(i).getDescripcion());
                data[5] = String.valueOf(Main.con.listaProducto.get(i).getCantStock());
                data[6] = String.valueOf(Main.con.listaProducto.get(i).getTipo());
                tableModel.addRow(data);
                if (Main.con.listaProducto.get(i).getEstado() == 1) {
                    tableModel.setValueAt(true, contador, 7);
                } else {
                    tableModel.setValueAt(false, contador, 7);
                }
                contador++;
            }
        }

        tableModel.fireTableDataChanged();
        panel.jTable.setModel(tableModel);
    }

    public class panelTablaProductos extends JPanel {

        JScrollPane scroll;
        JTable jTable;

        public panelTablaProductos() {
            this.setLayout(null);
            this.setBounds(20, 20, 400, 500);
            this.setBackground(Color.red);
            initJtable();
            setUpTableData();
            this.setVisible(true);
        }

        public void initJtable() {
            String[] colName = {"ID Producto", "ID Empresa", "Nombre", "Precio", "Descripción", "Cantidad", "Tipo", "Estado"};
            if (jTable == null) {
                jTable = new JTable() {

                    @Override
                    public Class getColumnClass(int column) {
                        switch (column) {
                            case 0:
                                return int.class;
                            case 1:
                                return int.class;
                            case 2:
                                return String.class;
                            case 3:
                                return int.class;
                            case 4:
                                return String.class;
                            case 5:
                                return int.class;
                            case 6:
                                return int.class;
                            default:
                                return Boolean.class;
                        }
                    }

                    @Override
                    public boolean isCellEditable(int nRow, int nCol) {
                        if (nCol == 7) {
                            return true;
                        }
                        return false;
                    }

                };
            }

            DefaultTableModel contactTableModel = (DefaultTableModel) jTable
                    .getModel();
            contactTableModel.setColumnIdentifiers(colName);
            jTable.addMouseListener(new MouseAdapter() {
                @Override
                public void mousePressed(MouseEvent e) {
                    if (e.getClickCount() == 1) {
                        try {
                            JTable target = (JTable) e.getSource();
                            Statement stmt = null;
                            Main.con.conSQL.setAutoCommit(false);

                            System.out.println("Opened database successfully");
                            stmt = Main.con.conSQL.createStatement();
                            if (target.getSelectedColumn() == 7) {
                                try {
                                    int row = target.getSelectedRow();
                                    boolean estado = (boolean) contactTableModel.getValueAt(row, 7);
                                    Producto p = new Producto();
                                    p.setIdProducto(Integer.parseInt((String) jTable.getValueAt(row, 0)));
                                    p.setIdEmpresa(Integer.parseInt((String) jTable.getValueAt(row, 1)));
                                    p.setNombre((String) jTable.getValueAt(row, 2));
                                    p.setPrecio(Integer.parseInt((String) jTable.getValueAt(row, 3)));
                                    p.setDescripcion((String) jTable.getValueAt(row, 4));
                                    p.setCantStock(Integer.parseInt((String) jTable.getValueAt(row, 5)));
                                    p.setTipo(Integer.parseInt((String) jTable.getValueAt(row, 6)));
                                    if (jTable.getValueAt(row, 7).equals(true)) {
                                        p.setEstado(0);
                                        ResultSet rs = stmt.executeQuery("select correos from obtenerCorreo" + "(" + p.getIdEmpresa() + ")");
                                        rs.next();
                                        String correo = rs.getString("correos");
                                        Main.con.conSQL.commit();

                                        notificacion(correo, p.getIdProducto(), p.getNombre(), p.getIdEmpresa());
                                    } else {
                                        p.setEstado(1);
                                    }

                                    stmt.executeQuery("select cambiarEstadoProducto" + "(" + p.getIdProducto() + "," + p.getIdEmpresa() + ")");
                                    Main.con.conSQL.commit();
                                    System.out.println("select cambiarEstadoProducto" + "(" + p.getIdProducto() + "," + p.getIdEmpresa() + ")");
                                    stmt.close();

                                } catch (SQLException ex) {
                                    System.err.println("Error");
                                    Logger.getLogger(MainWindow.class.getName()).log(Level.SEVERE, null, ex);
                                }
                            }
                        } catch (SQLException ex) {
                            Logger.getLogger(MainWindow.class.getName()).log(Level.SEVERE, null, ex);
                        }
                    }
                }

            });

            jTable.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
            jTable.setAutoResizeMode(JTable.AUTO_RESIZE_ALL_COLUMNS);
            jTable.getTableHeader().setReorderingAllowed(false);
            TableRowSorter<TableModel> elQueOrdena = new TableRowSorter<TableModel>(contactTableModel);
            jTable.setRowSorter(elQueOrdena);
            scroll = new JScrollPane(jTable);
            scroll.setBounds(0, 0, 400, 500);
            scroll.setVisible(true);
            this.add(scroll);
        }

        public void setUpTableData() {

            DefaultTableModel tableModel = (DefaultTableModel) jTable.getModel();
            Main.con.select();
            String[] data = new String[7];
            for (int i = 0; i < Main.con.listaProducto.size(); i++) {

                data[0] = String.valueOf(Main.con.listaProducto.get(i).getIdProducto());
                data[1] = String.valueOf(Main.con.listaProducto.get(i).getIdEmpresa());
                data[2] = String.valueOf(Main.con.listaProducto.get(i).getNombre());
                data[3] = String.valueOf(Main.con.listaProducto.get(i).getPrecio());
                data[4] = String.valueOf(Main.con.listaProducto.get(i).getDescripcion());
                data[5] = String.valueOf(Main.con.listaProducto.get(i).getCantStock());
                data[6] = String.valueOf(Main.con.listaProducto.get(i).getTipo());

                tableModel.addRow(data);
                if (Main.con.listaProducto.get(i).getEstado() == 1) {
                    tableModel.setValueAt(true, i, 7);
                } else {
                    tableModel.setValueAt(false, i, 7);
                }

                if (idEmpresasList.isEmpty()) {
                    idEmpresasList.add(Main.con.listaProducto.get(i).getIdEmpresa());
                } else {
                    if (!idEmpresasList.contains(Main.con.listaProducto.get(i).getIdEmpresa())) {
                        idEmpresasList.add(Main.con.listaProducto.get(i).getIdEmpresa());
                    }
                }
            }
            tableModel.fireTableDataChanged();
            jTable.setModel(tableModel);
            jTableG = jTable;
            jComboEmpresas = new JComboBox();
            jComboEmpresas.addItem("Todas");

            /**/
        }

    }

    public void notificacion(String correoDestino, int id, String nombre, int idEmpresa) {

        try {

            // Define message
            String mensaje = "Los siguentes productos fueron desactivados: " + "\n";
            mensaje = mensaje + "ID Empresa: " + idEmpresa + "\n";
            mensaje = mensaje + "codigo: " + id + ", ";
            mensaje = mensaje + "nombre: " + nombre + "\n";

            MimeMessage message = new MimeMessage(session);
            message.setFrom(new InternetAddress(username));
            message.setSubject("Articulos rechazados");
            message.addRecipient(Message.RecipientType.TO, new InternetAddress(correoDestino));

            message.setText(mensaje);
            // Envia el mensaje
            Transport.send(message);
        } catch (Exception e) {
            System.err.println("Error: " + e.toString());
        }

    }

    public void notificacionVarios(String correoDestino, ArrayList<Producto> listProducto) {

        try {

            // Define message
            String mensaje = "Los siguentes productos fueron desactivados: " + "\n";
            mensaje = mensaje + "ID Empresa: " + listProducto.get(0).getIdEmpresa() + "\n";
            for (int i = 0; i < listProducto.size(); i++) {
                mensaje = mensaje + "codigo: " + listProducto.get(i).getIdProducto() + ", ";
                mensaje = mensaje + "nombre: " + listProducto.get(i).getNombre() + "\n";
            }
            MimeMessage message = new MimeMessage(session);
            message.setFrom(new InternetAddress(username));
            message.setSubject("Articulos rechazados");
            message.addRecipient(Message.RecipientType.TO, new InternetAddress(correoDestino));

            message.setText(mensaje);
            // Envia el mensaje
            Transport.send(message);
        } catch (Exception e) {
            System.err.println("Error: " + e.toString());
        }

    }
}
