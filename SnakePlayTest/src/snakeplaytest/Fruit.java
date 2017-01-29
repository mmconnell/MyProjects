/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package snakeplaytest;

import java.awt.geom.Point2D;

/**
 *
 * @author Michael
 */
public class Fruit 
{
    private Point2D.Double pos;
    
    public Fruit(Point2D.Double pos)
    {
        this.pos = new Point2D.Double(pos.x, pos.y);
    }
    
    public Point2D.Double getPos()
    {
        return new Point2D.Double(pos.x, pos.y);
    }
}
