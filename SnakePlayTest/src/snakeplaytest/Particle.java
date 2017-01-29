/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package snakeplaytest;

import javafx.scene.paint.Color;

/**
 *
 * @author Michael
 */
public class Particle 
{
    private double x;
    private double y;
    private double size;
    private double max;
    private boolean rotate;
    private int timer;
    private int totalTimer;
    private int speed;
    
    public Particle(double xLoc, double yLoc, double size, double max, int speed)
    {
        this.x = xLoc;
        this.y = yLoc;
        this.size = size;
        this.max = max;
        this.rotate = false;
        this.totalTimer = 3;
        this.timer = 0;
        this.speed = speed;
    }
    
    public double[] coolMove()
    {
        double xStart, yStart, xEnd, yEnd;
        this.y += 1;
        if(rotate)
        {
            xStart = x + (size/2);
            yStart = y - (size/4);
            xEnd = x + (size/2);
            yEnd = y + size + (size/4);
        }
        else
        {
            xStart = x;
            yStart = y;
            xEnd = x + size;
            yEnd = y + size;
        }
        if(y + size > max)
        {
            return null;
        }
        rotate = !rotate;
        double[] array = {xStart, yStart, xEnd, yEnd};
        return array;
    }
    
    public double[][] move()
    {
        double x1, x2, x3, x4, y1, y2, y3, y4;
        if(timer == totalTimer)
        {
            rotate = !rotate;
            this.y += speed;
            timer = 0;
        }
        else
        {
            timer++;
        }
        if(rotate)
        {
            x1 = x + (size/2);
            x2 = x + size + size/4;
            x3 = x1;
            x4 = x - (size/4);
            y1 = y - (size/4);
            y2 = y + (size/2);
            y3 = y + size + (size/4);
            y4 = y2;
        }
        else
        {
            x1 = x;
            x2 = x + size;
            x3 = x + size;
            x4 = x;
            y1 = y;
            y2 = y;
            y3 = y + size;
            y4 = y + size;
        }
        if(y + size > max)
        {
            return null;
        }
        double[] xArray = {x1, x2, x3, x4, x1};
        double[] yArray = {y1, y2, y3, y4, y1};
        double[][] finalArray = new double[2][5];
        finalArray[0] = xArray;
        finalArray[1] = yArray;
        return finalArray;
    }
}
