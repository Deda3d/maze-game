using System.Net.NetworkInformation;
using static System.Console;
int x = 27; //размеры лабиринта по Х
int y = 27; //размеры лабиринта по У
CursorVisible = false;

int[] xused = new int[x * y]; //координаты Х точек, через которые лежат проходы (количество ячеек х*у потому что там очень много ходов, и нужно много места )
int[] yused = new int[x * y]; //координаты У точек, через которые лежат проходы (количество ячеек х*у потому что там очень много ходов, и нужно много места )
xused[0] = 2; //записываем координату Х начальной точки
yused[0] = 2; //записываем координату У начальной точки
int numused = 1; //счетчик для того, чтобы запоминать следующие точки
int randomused = 0; //количество переходов к случайной точке лабиринта после того, как возможность двигаться пропадает

int[,] lab = new int[y, x]; //сам лабиринт
int[,] used = new int[y, x]; //пустой массив, который будет заполняться единицами. 1 = мы уже были в этой точке

used[2, 2] = 1; //записываем начальную точку как использованную

int xp = 2; //координаты игрока по Х
int yp = 2; //координаты игрока по У

for (int i = 0; i < y; i++)
{
    for (int j = 0; j < x; j++) lab[i, j] = 1;
}//заполняем лабиринт стенами (1)
for (int i = 1; i < y - 1; i++)
{
    for (int j = 1; j < x - 1; j++) lab[i, j] = 0;
}//заполняем его внутри пустым пространством (0)
for (int i = 1; i < y - 1; i++)
{
    for (int j = 0; j < x - 1; j++) if (i % 2 == 1 || j % 2 == 1) lab[i, j] = 1;
}//делаем "сетку" чтобы нули были через 1 стену

for (int i = 0; i < 2; i++)
{
    for (int j = 0; j < x; j++) lab[i, j] = 2;
}//обозначаем верхнюю границу (2)
for (int i = y - 2; i < y; i++)
{
    for (int j = 0; j < x; j++) lab[i, j] = 2;
}//обозначаем нижнюю границу (2)
for (int j = 0; j < 2; j++)
{
    for (int i = 0; i < y; i++) lab[i, j] = 2;
}//обозначаем левую границу (2)
for (int j = x - 2; j < x; j++)
{
    for (int i = 0; i < y; i++) lab[i, j] = 2;
}//обозначаем правую границу (2)


Random rnd = new Random();
bool CanMove = true; //если "змейка" может дальше строить ход, то true, иначе false
while (CanMove == true)
{
    int nap = rnd.Next(1, 5); //генерируем случайное направление. 1 - вправо, 2 - вверх, 3 - влево, 4 - вниз

    if ((nap == 1) && (lab[yp, xp + 2] == 0) && (used[yp, xp + 2] != 1)) //если направление вправо И в правой клетке пустое пространство И "змейка" там еще не была, то
    {
        lab[yp, xp + 1] = 0; //"ломаем" стену справа
        xp += 2; //переходим в свободную клетку справа
        used[yp, xp] = 1; //находясь в новой клетке запоминаем, что мы в ней были, чтобы не попадать в неё снова
        xused[numused] = xp; //запоминаем координату Х этой точки, чтобы потом к ней можно было вернуться
        yused[numused] = yp; //запоминаем координату У этой точки, чтобы потом к ней можно было вернуться
        numused++; //переходим к следующей точке, но заполняться она уже будет не тут, просто выходим из прошлой, так как мы уже получили её Х и У
    }
    if ((nap == 4) && (lab[yp + 2, xp] == 0) && (used[yp + 2, xp] != 1)) //то же самое, что и для направления вправо(1) только теперь вниз(4)
    {
        lab[yp + 1, xp] = 0;
        yp += 2;
        used[yp, xp] = 1;
        xused[numused] = xp;
        yused[numused] = yp;
        numused++;
    }
    if ((nap == 3) && (lab[yp, xp - 2] == 0) && (used[yp, xp - 2] != 1)) //то же самое, что и для направления вправо(1) только теперь влево(3)
    {
        lab[yp, xp - 1] = 0;
        xp -= 2;
        used[yp, xp] = 1;
        xused[numused] = xp;
        yused[numused] = yp;
        numused++;
    }
    if ((nap == 2) && (lab[yp - 2, xp] == 0) && (used[yp - 2, xp] != 1)) //то же самое, что и для направления вправо(1) только теперь вверх(2)
    {
        lab[yp - 1, xp] = 0;
        yp -= 2;
        used[yp, xp] = 1;
        xused[numused] = xp;
        yused[numused] = yp;
        numused++;
    }

    if ((lab[yp + 2, xp] == 2 || used[yp + 2, xp] == 1) && (lab[yp - 2, xp] == 2 || used[yp - 2, xp] == 1) && (lab[yp, xp + 2] == 2 || used[yp, xp + 2] == 1) && (lab[yp, xp - 2] == 2 || used[yp, xp - 2] == 1))
    //если (снизу граница(2) ИЛИ снизу стена(1)) И (сверху граница(2) ИЛИ сверху стена(1)) И (справа граница(2) ИЛИ справа стена(1)) И (слева граница(2) ИЛИ слева стена(1)) ТО МЫ НЕ МОЖЕМ ДВИГАТЬСЯ, тогда:
    {
        int r = rnd.Next(1, numused); //берем случайную точку лабиринта
        xp = xused[r]; //переходим в неё, меняя кординату игрока по Х
        yp = yused[r]; //переходим в неё, меняя кординату игрока по У
        randomused++; //увеличиваем счетчик переходов к случайным точкам на 1;
        //таким образом после всего вышеперечисленного мы начинаем строить новую "змейку" со случайной точки, чтобы построить новые ходы в лабиринте
    }
    if (randomused == x * y) CanMove = false; //если мы уже достаточно много раз строили "змейку" в случайных точках, то, скорее всего, лабиринт уже построен. на самом деле тут можно точно посчитать нужно количество переходов, но так как лабиринт строится мгновенно, то можно взять просто площадь лабиринта как ограничение по переходам.
}


int[,] labprov = new int[y, x];
for (int i = 0; i < y; i++)
{
    for (int j = 0; j < x; j++)
    {
        labprov[i, j] = lab[i, j];
    }
}

xp = x - 3;
yp = y - 3;


bool prov = true;
while (prov == true)
{
    int nap = rnd.Next(1, 5);
    if (nap == 1 && labprov[yp, xp + 1] == 0)
    {
        labprov[yp, xp] = -1;
        xp++;
    }
    else if (nap == 3 && labprov[yp, xp - 1] == 0)
    {
        labprov[yp, xp] = -1;
        xp--;
    }
    else if (nap == 2 && labprov[yp - 1, xp] == 0)
    {
        labprov[yp, xp] = -1;
        yp--;
    }
    else if (nap == 4 && labprov[yp + 1, xp] == 0)
    {
        labprov[yp, xp] = -1;
        yp++;
    }
    if (xp == 2 && yp == 2)
    {
        labprov[yp, xp] = -1;
        prov = false;
    }
    else if (labprov[yp + 1, xp] != 0 && labprov[yp - 1, xp] != 0 && labprov[yp, xp + 1] != 0 && labprov[yp, xp - 1] != 0)
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                labprov[i, j] = lab[i, j];
            }
        }
        xp = x - 3;
        yp = y - 3;
    }

}

int nmin = Convert.ToInt32(Math.Sqrt(x * y));
while (nmin > 0)
{
    int minx = rnd.Next(2, x - 2);
    int miny = rnd.Next(2, x - 2);
    if (labprov[miny, minx] == 0)
    {
        lab[miny, minx] = -2;
        nmin--;
    }
}

lab[2, 2] = 6;
lab[y - 3, x - 3] = 9;
for (int i = 0; i < y; i++) //выводим лабиринт на экран
{
    for (int j = 0; j < x; j++)
    {
        if (lab[i, j] == 1) Console.ForegroundColor = ConsoleColor.Black; //черный цвет в случае, если это стена
        if (lab[i, j] == 0) Console.ForegroundColor = ConsoleColor.White; //белый цвет в случае, если это проход
        if (lab[i, j] == 2) Console.ForegroundColor = ConsoleColor.DarkGray; //серый цвет в случае, если это граница
        if (lab[i, j] == -1) Console.ForegroundColor = ConsoleColor.Green;
        if (lab[i, j] == -2) Console.ForegroundColor = ConsoleColor.Red;
        if (lab[i, j] == 6) Console.ForegroundColor = ConsoleColor.Blue;
        if (lab[i, j] == 9) Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("██");
        //Console.Write(labprov[i, j]); //в случае вывода чисел вместо цветных пикселей
        //Console.Write(' '); //в случае вывода чисел вместо цветных пикселей

    }
    Console.WriteLine();
}

xp = 2;
yp = 2;
int kursy = 2;
int kursx = 4;
bool winorloose = false;
while (winorloose == false)
{
    var ch = Console.ReadKey(true).Key;
    switch (ch)
    {
        case ConsoleKey.D:
            if (lab[yp, xp + 1] == 0)
            {
                lab[yp, xp] = 0;
                xp++;
                lab[yp, xp] = 6;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("██");
                kursx += 2;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("██");
            }
            else if (lab[yp, xp + 1] == -2)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lose");
                winorloose = true;
            }
            else if (lab[yp, xp + 1] == 9)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You win");
                winorloose = true;
            }
            break;
        case ConsoleKey.A:
            if (lab[yp, xp - 1] == 0)
            {
                lab[yp, xp] = 0;
                xp--;
                lab[yp, xp] = 6;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("██");
                kursx -= 2;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("██");
            }
            else if (lab[yp, xp - 1] == -2)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lose");
                winorloose = true;
            }
            else if (lab[yp, xp - 1] == 9)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You win");
                winorloose = true;
            }
            break;
        case ConsoleKey.W:
            if (lab[yp - 1, xp] == 0)
            {
                lab[yp, xp] = 0;
                yp--;
                lab[yp, xp] = 6;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("██");
                kursy--;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("██");
            }
            else if (lab[yp - 1, xp] == -2)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lose");
                winorloose = true;
            }
            else if (lab[yp - 1, xp] == 9)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You win");
                winorloose = true;
            }
            break;
        case ConsoleKey.S:
            if (lab[yp + 1, xp] == 0)
            {
                lab[yp, xp] = 0;
                yp++;
                lab[yp, xp] = 6;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("██");
                kursy++;
                Console.SetCursorPosition(kursx, kursy);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("██");
            }
            else if (lab[yp + 1, xp] == -2)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lose");
                winorloose = true;
            }
            else if (lab[yp + 1, xp] == 9)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You win");
                winorloose = true;
            }
            break;
    }
}