using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TestSuite
{
    //intanciacion de los atributos de la carpeta Resourse 
    GameObject playerPrefab = Resources.Load<GameObject>("Player");
    GameObject coinPrefab = Resources.Load<GameObject>("dollar");
    GameObject killPrefab = Resources.Load<GameObject>("Killzone");
    GameObject winPrefab = Resources.Load<GameObject>("LevelCompleteTrigger");

    [SetUp] public void SetUp() {
        SceneManager.LoadScene("Scenes/Level"); // instanciacion de la escena del Nivel
    }

    //Prueba de salto del personaje
    [UnityTest] public IEnumerator TestSalto() {
        //instanciacion del objeto Player para la prueba
        GameObject player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        yield return null;
        PlayerController playerController = player.GetComponent<PlayerController>();

        float initPosY = player.transform.position.y; //toma el valor inicial del jugador
        playerController.MobileJump(); // intancia el metodo salto del tipo movile
        yield return new WaitForSeconds(0.4f);
        float finalposY = player.transform.position.y; // toma el nuevo valor del jugador despues del salto
        Assert.That(finalposY,Is.GreaterThan(initPosY)); //si la nueva posicion es mayor a la posicion inicial, entonces es correcto
    }

    [UnityTest] public IEnumerator AgarrarMoneda() {
        //instanciacion del objeto Player para la prueba
        GameObject player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //instanciacion del objeto Coin para la prueba
        GameObject coin = GameObject.Instantiate(coinPrefab, new Vector3(1,0),Quaternion.identity);
        yield return null;
        PlayerController playerController = player.GetComponent<PlayerController>();

        int initCountCoin = GameManager.instance.CoinCount; //toma el valor inicial de la cantidad de monedas de la clase GameManager
        playerController.transform.position = coin.transform.position; //mueve al jugador a la posicion de Coin para que interactue
        yield return null;
        int finalCountCoin = GameManager.instance.CoinCount;//toma el nuevo valor de la cantidad de monedas de la clase GameManager

        Assert.That(finalCountCoin, Is.GreaterThan(initCountCoin)); //compara si la cantidad de Coin actual es mayor a la cantida de Coin inicial, de ser asi entonces se puede entender que si se agarro la moneda

    }

    [UnityTest] public IEnumerator ZonaMuerteJugador() {
        //instanciacion del objeto Player para la prueba
        GameObject player = GameObject.Instantiate(playerPrefab, Vector3.zero,Quaternion.identity) ;
        //instanciacion del objeto Killzone para la prueba
        GameObject killzone = GameObject.Instantiate(killPrefab, new Vector3(0, 2),Quaternion.identity);
        yield return null;
        PlayerController playerController = player.GetComponent<PlayerController>();

        playerController.transform.position = new Vector3(0.5f,0); //mueve al jugador para que caiga sobre el objeto Killzone e interactuen
        yield return null;

        Assert.That(GameManager.instance.PlayerController.isActiveAndEnabled, Is.False); // si el objeto player se desactiva de la escena entonces se puede considerar que Murio, por lo tanto que la prueba es correcta
    }

    [UnityTest] public IEnumerator GanarPartida() {
        //instanciacion del objeto Player para la prueba
        GameObject player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //instanciacion del objeto LevelCompleteTrigger o salida, para la prueba
        GameObject salida = GameObject.Instantiate(winPrefab, new Vector3(3,1), Quaternion.identity);
        yield return null;
        PlayerController playerController = player.GetComponent<PlayerController>();
        ExitTrigger exitTrigger = salida.GetComponent<ExitTrigger>();

        playerController.transform.position = salida.transform.position; //cambio la posicion de player a la posicion del objeto Salida para que interactuen;
        yield return new WaitForSeconds(2.5f);

        Assert.That(GameManager.instance.LevelCompletePanel.activeSelf, Is.True); //si el objeto de la clase GameManager se activa entonces se puede considerar que gano o paso el nivel, por lo tanto que la prueba es correcta
        yield return new WaitForSeconds(2f);
    }

}
