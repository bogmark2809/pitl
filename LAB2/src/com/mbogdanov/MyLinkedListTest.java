package com.mbogdanov;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

class MyLinkedListTest {
    @Test
    public void testAddOneItem() {
        MyLinkedList list = new MyLinkedList();
        list.add(3);
        assertEquals(3, list.get(0));
    }

    @Test
    public void testAddSecondItem() {
        MyLinkedList list = new MyLinkedList();
        list.add(3);
        list.add(5);
        assertEquals(5, list.get(1));
    }

    @Test
    public void testFirstItemRemainsAfterAddingSecond() {
        MyLinkedList list = new MyLinkedList();
        list.add(3);
        list.add(5);
        assertEquals(3, list.get(0));
    }

    @Test
    public void testFewItemRemains() {
        MyLinkedList list = new MyLinkedList();
        for (int i = 0; i < 15; i++)
            list.add(i);

        assertEquals(0, list.get(0));
        assertEquals(5, list.get(5));
        assertEquals(7, list.get(7));
        assertEquals(10, list.get(10));
        assertEquals(14, list.get(14));
    }

    @Test
    public void testFewStringRemains() {
        MyLinkedList list = new MyLinkedList();
        for (int i = 0; i < 15; i++)
            list.add(String.valueOf(i));

        assertEquals("0", list.get(0));
        assertEquals("5", list.get(5));
        assertEquals("7", list.get(7));
        assertEquals("10", list.get(10));
        assertEquals("14", list.get(14));
    }
}