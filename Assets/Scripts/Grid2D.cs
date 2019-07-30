using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid2D : MonoBehaviour
{
    //Declaracion de variables
    public int width; //Ancho de la matrix 
    public int height; //Altura de la matriz
    public GameObject puzzlePiece; //Declaraicon del objeto (Pelotas)
    public GameObject[,] grid; //Declaracion de la matriz
    public bool isPlayerOne; //Booleano que controla el jugado activo

    public int rojo; //Variable que guarda las consecutivas rojas 
    public int azul; //Variable que guarda las consecutivas azules
    private Color tinte; //variable que guarda el color de la pelota activa para conteo
    
    //Variables para la verificacion del numero de pelotas consecutivas
    public int conth; 
    public int contv;
    public int contk;
    public int contp;
    
    //Variables para control de texto en la pantalla
    public Text Rojo;
    public Text Azul;
    //Variables para conteo y display de victorias de los jugadores
    public Text numero;
    public Text numero2;
    public int num;
    public int num2;
    
    //Variable que controla el fin y reinicio del juego 
    private bool termino;
	


    // Use this for initialization
    void Start()
    {
        
        grid = new GameObject[width, height];//Creacion de la matriz

        for (int x = 0; x < width; x++)  //Ciclo para la instanciacion de las pelotas
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = GameObject.Instantiate(puzzlePiece) as GameObject;
                Vector3 position = new Vector3(x, y, 0);
                go.transform.position = position;
                grid[x, y] = go;
                
            }
        }
        isPlayerOne = true; //Booleano para controlar quien es el jugador activo
        Azul.text = ""; //Control del texto del jugador activo
        termino = false; //Booleano que se hace falso para permitir el juego
        Destru(grid); //Llamada al metodo para destruir pelotas (Regla extra) 
    }

    // Update is called once per frame
    void Update()
    {
        if (termino) //Ciclo para pausar el juego por cierto tiempo una vez alguien gana
        {
            return;
        }
        else
        {
            Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Localizador de la posicion del mouse en la pantalla
            Debug.DrawLine(Vector3.zero, mPosition);
            UpdatePickedPiece(mPosition); //Llamada al metodo de iluminacion y escogencia de las pelotas
        }
    }

    public void UpdatePickedPiece(Vector3 position)
    {
        int x = (int)(position.x + 0.5f);
        int y = (int)(position.y + 0.5f);

        for (int _x = 0; _x < width; _x++) //Ciclo que pinta las pelotas de blanco si su color es diferente a rojo o azul
        {
            for (int _y = 0; _y < height; _y++)
            {
                
                {
                    GameObject go = grid[_x, _y];
                    if (go.GetComponent<Renderer>().material.color != Color.red && go.GetComponent<Renderer>().material.color != Color.blue)
                    {
                        go.GetComponent<Renderer>().material.SetColor("_Color", Color.white);


                    }
                   
                }
            }
        }

        if (x >= 0 && y >= 0 && x < width && y < height)
        {


            Senalar(grid[x, y]); //Llamada al metodo que ilumina las pelotas temporalmente

            if (Input.GetMouseButtonDown(0)) //Condicional que controla los efectos al hacer click en una pelota
            {
                GameObject go = grid[x, y];

                if (go.GetComponent<Renderer>().material.color != Color.red && go.GetComponent<Renderer>().material.color != Color.blue)
                {
                    ColorFijo(x, y, go); //Llamada al metodo que fija un color permanentemente en una pelota clickeada

                    //Llamada a los metodos que verifican horizontalmente el numero de pelotas consecutivas del mismo color.
                    VerIzq(x, y, go.GetComponent<Renderer>().material.color);
                    VerDer(x, y, go.GetComponent<Renderer>().material.color);
                    //Llamada a los metodos que verifican verticalmente el numero de pelotas consecutivas del mismo color.
                    VerArr(x, y, go.GetComponent<Renderer>().material.color);
                    VerAba(x, y, go.GetComponent<Renderer>().material.color);
                    //Llamada a los metodos que verifican en las diagonales a la derecha el numero de pelotas consecutivas del mismo color.
                    DiagDerU(x, y, go.GetComponent<Renderer>().material.color);
                    DiagDerD(x, y, go.GetComponent<Renderer>().material.color);
                    //Llamada a los metodos que verifican en las diagonales a la izquierda el numero de pelotas consecutivas del mismo color.
                    DiagIzqU(x, y, go.GetComponent<Renderer>().material.color);
                    DiagIzqD(x, y, go.GetComponent<Renderer>().material.color);
                    //Suma de las consecutivas en las 4 direcciones (cada una por separado)
                    conth = VerIzq(x, y, go.GetComponent<Renderer>().material.color) + VerDer(x, y, go.GetComponent<Renderer>().material.color);//Horizontal
                    contv = VerArr(x, y, go.GetComponent<Renderer>().material.color) + VerAba(x, y, go.GetComponent<Renderer>().material.color);//Vertical
                    contk = DiagDerU(x, y, go.GetComponent<Renderer>().material.color) + DiagIzqD(x, y, go.GetComponent<Renderer>().material.color);//De izq abajo a der arriba
                    contp = DiagIzqU(x, y, go.GetComponent<Renderer>().material.color) + DiagDerD(x, y, go.GetComponent<Renderer>().material.color);//De der arriba a Izq abajo

                    if (conth == 3 || contv == 3 || contk == 3 || contp == 3) //Condicional que anuncia quien gano
                    {
                        if (isPlayerOne)
                        {
                            Debug.Log("Gano el Azul");
                            num++;
                            
                            numero.text = num.ToString(); //String para conteo de victorias del rojo
                            StartCoroutine(TiempoEsp()); //Llamada  a la corrutina para esperar un tiempo antes de reiniciar el juego.
                        }
                        else
                        {
                            Debug.Log("Gano el Rojo");
                            num2++;
                            numero2.text = (num2.ToString()); //String para conteo de vistorias del azul
                            StartCoroutine(TiempoEsp()); //Llamada  a la corrutina para esperar un tiempo antes de reiniciar el juego.
                        }
                       
                    }
                }
                

                

            }
        }

    }
    IEnumerator TiempoEsp() //Corrutina que hace esperar unos segundos al finalizar el juego antes de reiniciarlo.
    {
        termino = true; //Booleano que controla los efectos de fin del juego
        yield return new WaitForSeconds(5f);
        
        Blanco(grid); //Llamada al metodo que repinta las pelotas de blanco
        Destru(grid); //Llamada al metodo que destruye pelotas (regla extra)
        termino = false;
    }

    void Senalar(GameObject turi) //Metodo que se encarga de iluminar temporalmente la pelota en la que el cursor esta encima
    {
        if (turi.GetComponent<Renderer>().material.color == Color.white)
        {
            turi.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

   public void ColorFijo (int x, int y, GameObject go) //Metodo que fija un color permanentemente en una pelota clickeada
    {
        
        if (isPlayerOne) //Para el jugador 1, rojo
        {
            if (go.GetComponent<Renderer>().material.color != Color.red && go.GetComponent<Renderer>().material.color != Color.blue)
            {
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                
            }
  
        }
        else //Para el jugador 2, azul
        {
            if (go.GetComponent<Renderer>().material.color != Color.red && go.GetComponent<Renderer>().material.color != Color.blue)
            {
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                
            }
       
        }
        Suich();//Metodo para controlar el jugador activo
        
    }


    int VerIzq(int x, int y, Color tinte) //Metodo para verificar hacia la izquierda las pelotas consecutivas del mismo color.
    {
        int conth = 0;
        while (x > 0)
        {
            x = x - 1;
            if(grid[x,y].GetComponent<Renderer>().material.color == tinte)
            {
                conth++;
            }
            else
            {
                break;
            }
        }
        return conth;
        
            
    }
    int VerDer ( int x, int y, Color tinte) //Metodo para verificar hacia la derecha las pelotas consecutivas del mismo color.
    {
        int contk = 0;
        while (x < width-1)
        {
            x = x + 1;
            if (grid[x, y].GetComponent<Renderer>().material.color == tinte)
            {
                contk++;
            }
            else
            {
                break;
            }
        }
        return contk;
    }
    int VerArr(int x, int y, Color tinte) //Metodo para verificar hacia arriba las pelotas consecutivas del mismo color.
    {
        int contv = 0;
        while (y > 0)
        {
            y = y - 1;
            if (grid[x, y].GetComponent<Renderer>().material.color == tinte)
            {
                contv++;
            }
            else
            {
                break;
            }
        }
        return contv;
    }
    int VerAba(int x, int y, Color tinte) //Metodo para verificar hacia abajo las pelotas consecutivas del mismo color.
    {
        int contv = 0;
        while (y < height-1)
        {
            y = y + 1;
            if (grid[x, y].GetComponent<Renderer>().material.color == tinte)
            {
                contv++;
            }
            else
            {
                break;
            }
        }
        return contv;
    }
    int DiagDerU(int x, int y, Color tinte) //Metodo para verificar hacia la derecha - arriba las pelotas consecutivas del mismo color.
    {
        int contk = 0;
        while (y < height-1 && x < width-1)
        {
            x = x + 1;
            y = y + 1;
            if (grid[x, y].GetComponent<Renderer>().material.color == tinte)
            {
                contk++;
            }
            else
            {
                break;
            }
        }
        return contk;
    }
    int DiagIzqD(int x, int y, Color tinte) //Metodo para verificar hacia la izquierda - abajo las pelotas consecutivas del mismo color.
    {
        int contk = 0;
        while (y > 0 && x > 0)
        {
            x = x - 1;
            y = y - 1;
            if (grid[x, y].GetComponent<Renderer>().material.color == tinte)
            {
                contk++;
            }
            else
            {
                break;
            }
        }
        return contk;
    }
    int DiagIzqU(int x, int y, Color tinte) //Metodo para verificar hacia la izquierda - arriba las pelotas consecutivas del mismo color.
    {
        int contp = 0;
        while (y < height-1 && x > 0)
        {
            x = x - 1;
            y = y + 1;
            if (grid[x, y].GetComponent<Renderer>().material.color == tinte)
            {
                contp++;
            }
            else
            {
                break;
            }
        }
        return contp;
    }
    int DiagDerD(int x, int y, Color tinte) //Metodo para verificar hacia la izquierda - abajo las pelotas consecutivas del mismo color.
    {
        int contp = 0;
        while (y > 0 && x < width-1)
        {
            x = x + 1;
            y = y - 1;
            if (grid[x, y].GetComponent<Renderer>().material.color == tinte)
            {
                contp++;
            }
            else
            {
                break;
            }
        }
        return contp;
    }

	void Blanco(GameObject [,] grid) //Metodo para repintar de blanco las pelotas al reiniciar el juego
    {
        
        for (int a = 0; a < grid.GetLength(0); a++)
        {
            for (int b = 0; b < grid.GetLength(1); b++)
            {
                grid[a, b].GetComponent<Renderer>().material.color = Color.white;
                grid[a, b].SetActive(true); //Activacion de las pelotas desactivadas por el metodo destru
            }
        }
    }

    void Destru(GameObject [,] grid) //Metodo para destruir hasta 2 pelotas por fila (Regla Extra)
    {
        for (int a = 0; a < grid.GetLength(0); a++)
        {
            List<int> crash; //Declaracion de una lista
            crash = new List<int>();
            int c = Random.Range(0, 2); //Randomizador que controla cuantas pelotas por fila se van a destruir, o si no se va a destruir ninguna

            if (c == 1) //Condicional para destruir 1 pelota 
            {
                crash.Add(Random.Range(0, grid.GetLength(1))); 
                grid[a, crash[0]].SetActive(false);
            }
            else if (c == 2) //Condicional para destruir 2 pelotas
            {
                crash.Add(Random.Range(0, grid.GetLength(1)));
                int d = Random.Range(0, grid.GetLength(1));
                while (d == crash[0])
                {
                    d = Random.Range(0, grid.GetLength(1));
                }
                crash.Add(d);
                grid[a, crash[0]].SetActive(false);
                grid[a, crash[1]].SetActive(false);
            }
        }
    }
    void Suich() //Metodo que controla el jugador activo
    {
        isPlayerOne = !isPlayerOne; //Switch para cambiar de jugador una vez se da click
        if (isPlayerOne)
        {
            Azul.text = "";
            Rojo.text = "Rojo";
        }
        else
        {
            Rojo.text = "";
            Azul.text = "Azul";
        }
    }

}

