/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package snakeplaytest;

import java.awt.geom.Point2D;
import javafx.scene.paint.Color;

/**
 *
 * @author Michael
 */
public class SnakeSegment 
{
    private Point2D.Double position;
    private Direction direction;
    private Color color;
    
    public SnakeSegment(double x, double y, Direction dir, Color color)
    {
        position = new Point2D.Double(x, y);
        direction = dir;
        this.color = color;
    }
    
    public void setDirection(Direction dir)
    {
        direction = dir;
    }
    
    public Direction getDirection()
    {
        return direction;
    }
    
    public double getX()
    {
        return position.x;
    }
    
    public double getY()
    {
        return position.y;
    }
    
    public void move()
    {
        position.x = position.x + Direction.moveX(direction);
        position.y = position.y + Direction.moveY(direction);
    }
    
    public Color getColor()
    {
        return this.color;
    }
    
    public void setColor(Color color)
    {
        this.color = color;
    }
}
