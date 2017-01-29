/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package snakeplaytest;

import javafx.animation.AnimationTimer;
import javafx.application.Application;
import static javafx.application.Application.launch;
import javafx.application.Platform;
import javafx.geometry.HPos;
import javafx.geometry.Pos;
import javafx.geometry.VPos;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Label;
import javafx.scene.control.Menu;
import javafx.scene.control.MenuBar;
import javafx.scene.control.MenuItem;
import javafx.scene.control.ToolBar;
import javafx.scene.input.KeyCode;
import javafx.scene.input.KeyCodeCombination;
import javafx.scene.input.KeyCombination;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.scene.media.AudioClip;
import javafx.stage.Stage;

/**
 *
 * @author Michael
 */
public class SnakePlayTest extends Application{
    private Label mStatus;
    private AnimationTimer mTimer;
    private long milli;
    private SnakeMap map;
    private SevenSegment ones, tens;
    private boolean check = false;
    private AudioClip hit = new AudioClip(getClass().getResource("bite.wav").toString());
    private AudioClip song = new AudioClip(getClass().getResource("song.mp3").toString());
    
    @Override
    public void start(Stage primaryStage) {
        
        BorderPane root = new BorderPane();
        VBox innerBox = new VBox();
        HBox lowerBox = new HBox();
        
        song.setVolume(hit.getVolume()/5);
        map = new SnakeMap(primaryStage.heightProperty(), primaryStage.widthProperty());
        root.setCenter(innerBox);
        innerBox.getChildren().addAll(map, lowerBox);
        root.setTop(buildMenus());
        innerBox.setAlignment(Pos.CENTER);
        innerBox.prefWidthProperty().bind(map.widthProperty());
        innerBox.prefHeightProperty().bind(map.heightProperty());
        
        lowerBox.prefHeightProperty().bind(primaryStage.heightProperty().divide(2));
        tens = new SevenSegment(0, lowerBox.heightProperty().divide(1), lowerBox.widthProperty().divide(10), HPos.RIGHT, VPos.TOP);
        ones = new SevenSegment(0, lowerBox.heightProperty().divide(1), lowerBox.widthProperty().divide(10), HPos.LEFT, VPos.TOP);
        lowerBox.getChildren().addAll(tens, ones);
        lowerBox.setAlignment(Pos.TOP_CENTER);
        
        mStatus = new Label("Everything is Copacetic");
        ToolBar toolBar = new ToolBar(mStatus);
        root.setBottom(toolBar);
        
        Scene scene = new Scene(root, 800, 700);
        
        milli = System.currentTimeMillis();
        mTimer = new AnimationTimer() {
            @Override
            public void handle(long now) {
                if(map.okToRun() && checkTimer())
                {
                    milli = System.currentTimeMillis();
                    onRun();
                }
            }
        };
        
        primaryStage.setTitle("Hello World!");
        primaryStage.setScene(scene);
        primaryStage.show();
        
        mTimer.start();
    }
    
    private boolean checkTimer()
    {
        return System.currentTimeMillis() - milli > 17;
    }
    
    private void onRun()
    {
        if(!song.isPlaying())
        {
            song.stop();
            song.play();
        }
        
        if(map.lost())
        {
            if(!check)
            {
                check = true;
                ones.setNum(0);
                tens.setNum(0);
                ones.draw();
                tens.draw();
                displayScore();
            }
        }
        if(map.fruitEaten())
        {
            hit.stop();
            hit.play();   
            incrementScore();
            map.generateFruitLocation();
            map.setFruitEaten(false);
        }
        map.draw();
    }
    
    private void incrementScore()
    {
        if(ones.getNum() + 1 == 10)
        {
            tens.setNum((tens.getNum()+1)%10);
        }
        ones.setNum((ones.getNum()+1)%10);
        tens.draw();
        ones.draw();
    }
    
    private void setStatus(String status)
    {
        mStatus.setText(status);
    }
    
    private MenuBar buildMenus()
    {
        MenuBar menuBar = new MenuBar();
        
        Menu fileMenu = new Menu("_File");
        MenuItem quitMenuItem = new MenuItem("_Quit");
        quitMenuItem.setAccelerator(new KeyCodeCombination(KeyCode.Q, 
                KeyCombination.CONTROL_DOWN));
        quitMenuItem.setOnAction(actionEvent -> Platform.exit());
        fileMenu.getItems().add(quitMenuItem);
        
        Menu helpMenu = new Menu("_Help");
        MenuItem aboutMenuItem = new MenuItem("_About");
        aboutMenuItem.setOnAction(actionEvent -> onAbout());
        helpMenu.getItems().add(aboutMenuItem);
        menuBar.getMenus().addAll(fileMenu, helpMenu);
        return menuBar;
    }
    
    private void displayScore()
    {
        map.setOkToRun(false);
        Alert alert;
        alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setTitle("Score");
        alert.setHeaderText("Your score is : " + map.score());
        check = true;
        Platform.runLater( () -> showAlert(alert));
    }
    
    private void showAlert(Alert alert)
    {
        map.stopGame();
        map.startSnake();
        alert.showAndWait();
        map.setOkToRun(true);
        check = false;
    }
    
    private void onAbout()
    {
        map.setOkToRun(false);
        Alert alert;
        alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setTitle("About");
        alert.setHeaderText("Michael J McConnell, CSCD 370 Lab 0, Spring 2016");
        Platform.runLater( () -> showAlert(alert));
    }
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        launch(args);
    }
}
