/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package snakeplaytest;

import java.awt.geom.Point2D;
import java.util.ArrayList;
import java.util.Random;
import javafx.beans.property.ReadOnlyDoubleProperty;
import javafx.geometry.HPos;
import javafx.geometry.VPos;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.input.KeyEvent;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.Region;
import javafx.scene.paint.Color;

/**
 *
 * @author Michael
 */
public class SnakeMap extends Region
{
    private static final double widthX = 63;
    private static final double heightY = 63;
    private static final double shift = 3;
    private static final double rad = 3;
    private static final int totalBlocks = 18;
    private static final int transitTime = 3;
    private double scale;
    private Canvas mCanvas;
    private ArrayList<SnakeSegment> head;
    private Direction direction;
    private Fruit fruit;
    private boolean fruitEaten;
    private boolean canChange;
    private boolean okToRun = true;
    private boolean lost = false;
    private boolean okToMove = false;
    private static final int initSize = 6;
    private int score;
    private int xBlocks, yBlocks;
    public static final double transition = shift/transitTime;
    private int curTransitTime;
    private boolean inTransit = false;
    private Direction currentDirection;
    private ArrayList<Particle> particleList;
    private Random rand;
    private Color[] particleColorChance;
    private Color wallColor;
    private int wallCount = 0;
    
    public SnakeMap(ReadOnlyDoubleProperty heightProperty, ReadOnlyDoubleProperty widthProperty)
    {
        super();
        rand = new Random();
        this.mCanvas = new Canvas(widthX, heightY);
        mCanvas.setFocusTraversable(true);
        this.getChildren().add(mCanvas);
        this.setPrefHeight(1000);
        this.setPrefWidth(1000);
        this.prefHeightProperty().bind(heightProperty);
        this.prefWidthProperty().bind(widthProperty);
        mCanvas.addEventHandler(KeyEvent.KEY_PRESSED, keyEvent -> onButtonPressed(keyEvent));
        mCanvas.addEventHandler(MouseEvent.MOUSE_CLICKED, mouseEvent -> onMousePressed());
        particleList = new ArrayList<>();
        particleColorChance = generateListOfColors();
        startSnake();
        generateFruitLocation();
    }
    
    private Color[] generateListOfColors()
    {
        Color[] colors = {Color.CRIMSON, Color.CHOCOLATE, Color.DIMGRAY, Color.AQUAMARINE, Color.CADETBLUE, 
            Color.CORAL, Color.CORNFLOWERBLUE, Color.GOLDENROD, Color.LIGHTSLATEGRAY};
        return colors;
    }
    
    private void onMousePressed()
    {
        okToMove = !okToMove;
    }
    
    public int size()
    {
        return head.size();
    }
    
    public boolean okToRun()
    {
        return okToRun;
    }
    
    public void setOkToRun(boolean value)
    {
        okToRun = value;
    }
    
    public void stopGame()
    {
        okToMove = false;
        okToRun = false;
    }
    
    private void onButtonPressed(KeyEvent e)
    {
        okToMove = true;
        if(canChange)
        {
            canChange = false;
            switch (e.getCode()) {
                case DOWN:
                    setDirection(Direction.DOWN);
                    break;
                case UP:
                    setDirection(Direction.UP);
                    break;
                case RIGHT:
                    setDirection(Direction.RIGHT);
                    break;
                case LEFT:
                    setDirection(Direction.LEFT);
                    break;
                default:
                    canChange = true;
            }
        } 
    }
    
    private void generateParticle()
    {
        double width = (((double)rand.nextInt(3)) + 1.0)/4.0;//.25;
        double x = (int)rad + (rand.nextInt((int)(widthX-rad*2)/(int)shift)*(int)shift);
        int speed = rand.nextInt(3) + 1;
        particleList.add(new Particle(x, 0, width, heightY - 3, speed));
    }
    
    public boolean canChange()
    {
        return canChange;
    }
    
    public void giveChange()
    {
        canChange = true;
    }
    
    public void startSnake()
    {
        okToMove = false;
        xBlocks = initSize - 1;
        yBlocks = 0;
        this.head = new ArrayList();
        this.head.add(new SnakeSegment(rad*initSize, rad, Direction.RIGHT, Color.CRIMSON));
        this.direction = Direction.RIGHT;
        this.currentDirection = Direction.RIGHT;
        this.curTransitTime = 0;
        this.inTransit = false;
        this.particleList.clear();
        for(int x = 0; x < initSize - 1; x++)
        {
            add();
        }
        canChange = true;
    }
    
    public void generateFruitLocation()
    {
        Point2D.Double pos;
        do
        {
            double x = (int)rad + (rand.nextInt((int)(widthX-rad*2)/(int)shift)*(int)shift);
            double y = (int)rad + (rand.nextInt((int)(heightY-rad*2)/(int)shift)*(int)shift);
            pos = new Point2D.Double(x,y);
        }while(conflict(pos));
        fruit = new Fruit(pos);
    }
    
    private void move()
    {
        if(!inTransit)
        {
            for(int x = head.size() - 1; x > 0; x--)
            {
                head.get(x).setDirection(head.get(x-1).getDirection());
            }
            head.get(0).setDirection(direction);
            currentDirection = direction;
            curTransitTime = 0;
            inTransit = true;
        }
        curTransitTime++;
        if(!okToAlterBlocks())
        {
            return;
        }
        for(SnakeSegment point : this.head)
        {
            point.move();
        }
        
        if(curTransitTime == transitTime)
        {
            alterBlockCounts();
            rippleColor();
            inTransit = false;
        }
    }
    
    private void rippleColor()
    {
        for(int x = head.size() - 1; x > 0; x--)
        {
            head.get(x).setColor(head.get(x-1).getColor());
        }
    }
    
    private boolean okToAlterBlocks()
    {
        int originalY = yBlocks;
        int originalX = xBlocks;
        switch(currentDirection)
        {
            case UP:
                yBlocks--;
                break;
            case DOWN:
                yBlocks++;
                break;
            case RIGHT:
                xBlocks++;
                break;
            case LEFT:
                xBlocks--;
                break;
        }
        if(collision() || outOfBounds())
        {
            return false;
        }
        yBlocks = originalY;
        xBlocks = originalX;
        return true;
    }
    
    private void alterBlockCounts()
    {
        switch(currentDirection)
        {
            case UP:
                yBlocks--;
                break;
            case DOWN:
                yBlocks++;
                break;
            case RIGHT:
                xBlocks++;
                break;
            case LEFT:
                xBlocks--;
                break;
        }
    }
    
    public boolean collision()
    {
        for(int x = 1; x < this.head.size(); x++)
        {
            if(head.get(0).getX() == head.get(x).getX() && head.get(0).getY() == head.get(x).getY())
            {
                return true;
            }
        }
        return false;
    }
    
    public Color randomColor()
    {
        return particleColorChance[rand.nextInt(particleColorChance.length)];
    }
    
    public double getRad()
    {
        return rad;
    }
    
    public void add()
    {
        SnakeSegment s = head.get(head.size() - 1);
        Color color = randomColor();
        Color oldColor = head.get(0).getColor();
        while(color.equals(oldColor))
        {
            color = randomColor();
        }
        head.get(0).setColor(color);
        switch(s.getDirection())
        {
            case UP:
                head.add(new SnakeSegment(s.getX(), s.getY() + rad, s.getDirection(), s.getColor()));
                break;
            case DOWN:
                head.add(new SnakeSegment(s.getX(), s.getY() - rad, s.getDirection(), s.getColor()));
                break;
            case RIGHT:
                head.add(new SnakeSegment(s.getX() - rad, s.getY(), s.getDirection(), s.getColor()));
                break;
            case LEFT:
                head.add(new SnakeSegment(s.getX() + rad, s.getY(), s.getDirection(), s.getColor()));
                break;
        }
    }
    
    private boolean opposite(Direction direction)
    {
        switch(this.currentDirection)
        {
            case UP:
                return direction == Direction.DOWN;
            case DOWN:
                return direction == Direction.UP;
            case RIGHT:
                return direction == Direction.LEFT;
            case LEFT:
                return direction == Direction.RIGHT;
        }
        return false;
    }
    
    public void setDirection(Direction direction)
    {
        if(!opposite(direction))
        {
            this.direction = direction;
        }
    }
    
    @Override
    public void layoutChildren()
    {
        super.layoutChildren();
        
        double xValue = getWidth();
        double yValue = getHeight();
        double square = Math.min(xValue, yValue);
        mCanvas.setWidth(square);
        mCanvas.setHeight(square);
        this.layoutInArea(mCanvas, 0, 0, xValue, yValue, 0, HPos.CENTER, VPos.CENTER);
    }
    
    public int score()
    {
        return score;
    }
    
    private boolean outOfBounds()
    {
        Point2D.Double point = getLocation();
      
        return xBlocks < 0 || yBlocks < 0 || xBlocks > totalBlocks || yBlocks > totalBlocks;
    }
    
    private Point2D.Double getLocation()
    {
        return new Point2D.Double(this.head.get(0).getX(), this.head.get(0).getY());
    }
    
    public double getDistanceRatio()
    {
        double x = head.get(0).getX();
        double y = head.get(0).getY();
        double fruitX, fruitY;
        if(fruit == null)
        {
            fruitX = x;
            fruitY = y;
        }
        else
        {
            fruitX = fruit.getPos().x;
            fruitY = fruit.getPos().y;
        }
        
        double maxX = widthX - 3;
        double maxY = heightY - 3;
        
        double xSquare = Math.pow(x - fruitX, 2);
        double ySquare = Math.pow(y - fruitY, 2);
        
        double fruitDist = Math.sqrt(xSquare + ySquare);
        
        xSquare = Math.pow(x - maxX, 2);
        ySquare = Math.pow(y - maxY, 2);
        
        double totalDist = Math.sqrt(xSquare + ySquare);
        
        return fruitDist/totalDist;
    }
    
    public void draw()
    {
        lost = false;
        
        if(fruit == null)
        {
            generateFruitLocation();
        }
        GraphicsContext g = mCanvas.getGraphicsContext2D();
        
        g.clearRect(0, 0, mCanvas.getWidth(), mCanvas.getHeight());
        
        g.save();
        scale = mCanvas.getWidth()/widthX;
        g.scale(scale, scale);
        double normal = g.getLineWidth();
        g.setFill(Color.BEIGE);
        g.fillRect(0, 0, widthX, heightY);
        
        for(int x = 0; x < particleList.size(); x++)
        {
            double[][] arr = particleList.get(x).move();
            if(arr == null)
            {
                particleList.remove(x);
                x--;
            }
            else
            {
                double[] arrX = arr[0];
                double[] arrY = arr[1];
                g.setFill(particleColorChance[rand.nextInt(particleColorChance.length)]);//particleList.get(x).getColor());
                g.fillPolygon(arrX, arrY, arrX.length);
            }
        }
//        for(int x = 0; x < particleList.size(); x++)
//        {
//            double[] arr = particleList.get(x).coolMove();
//            if(arr == null)
//            {
//                particleList.remove(x);
//                x--;
//            }
//            else
//            {
//                g.setFill(particleColorChance[rand.nextInt(particleColorChance.length)]);//particleList.get(x).getColor());
//                g.fillRect(arr[0], arr[1], arr[2], arr[3]);
//            }
//        }
        
//        if(wallCount == 5)
//        {
//            wallCount = 0;
//            wallColor = Color.CRIMSON;//particleColorChance[rand.nextInt(particleColorChance.length)];
//        }
        g.setStroke(Color.CRIMSON);
        //wallCount++;

        g.setLineWidth(rad*2);
        g.strokeRect(0, 0, widthX, heightY);
        
        g.setLineWidth(normal);
        g.setFill(Color.STEELBLUE);
        g.fillOval(fruit.getPos().x, fruit.getPos().y, rad, rad);
        
        //g.setFill(Color.BLACK);
        g.setFill(wallColor);
        for(int x = 0; x < head.size(); x++)
        {
            SnakeSegment point = head.get(x);
            if(x != 0 && point.getDirection() != head.get(x - 1).getDirection())
            {
                double fromX = point.getX();
                double fromY = point.getY();
                double toY = rad;
                double toX = rad;
                double alter = shift - curTransitTime;
//                switch(point.getDirection())
//                {
//                    case UP:
//                        fromY -= alter;
//                        toY += alter;
//                        break;
//                    case DOWN:
//                        toY += alter;
//                        break;
//                    case RIGHT:
//                        toX += alter;
//                        break;
//                    case LEFT:
//                        fromX -= alter;
//                        toX += alter;
//                        break;
//                }
                g.setFill(head.get(x).getColor());
                g.fillRect(fromX, fromY, toX, toY);
            }
            else
            {
                g.setFill(head.get(x).getColor());
                g.fillRect(point.getX(), point.getY(), rad, rad);
            }
        }
        
        if(rand.nextInt(5) == 0)
        {
            generateParticle();
        }
        
        g.restore();
        canChange = true;
        if(okToMove && (outOfBounds() || collision()))
        {
            lost = true;
            score = head.size() - initSize;
            g.setFill(Color.DARKRED);
            g.fillRect(0, 0, mCanvas.getWidth(), mCanvas.getHeight());
            generateFruitLocation();
            startSnake();
        }
        if(okToMove)
        {
            move();
        }
        if(hitFruit())
        {
            wallColor = particleColorChance[rand.nextInt(particleColorChance.length)];
            fruitEaten = true;
            add();
        }
    }
    
    public boolean lost()
    {
        return lost;
    }
    
    public boolean fruitEaten()
    {
        return fruitEaten;
    }
    
    public void setFruitEaten(boolean value)
    {
        fruitEaten = value;
    }
    
    public boolean conflict(Point2D.Double pos)
    {
        for(SnakeSegment check : head)
        {
            if(pos.x == check.getX() && pos.y == check.getY())
            {
                return true;
            }
        }
        return false;
    }
    
    public boolean hitFruit()
    {
        return hit(fruit.getPos());
    }
    
    public boolean hit(Point2D.Double pos)
    {
        return pos.x == head.get(0).getX() && pos.y == head.get(0).getY();
    }
    
//    private enum Direction
//    {
//        UP, LEFT, RIGHT, DOWN;
//
//        public static int moveX(Direction dir)
//        {
//            int total = 0;
//            switch(dir)
//            {
//                case LEFT:
//                    total -= transition;
//                    break;
//                case RIGHT:
//                    total += transition;
//                    break;
//            }
//            return total;
//        }
//
//        public static int moveY(Direction dir)
//        {
//            int total = 0;
//            switch(dir)
//            {
//                case DOWN:
//                    total += transition;
//                    break;
//                case UP:
//                    total -= transition;
//                    break;
//            }
//            return total;
//        }
//    }
}
