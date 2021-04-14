import math
OFFSET = 268435456 # half of the earth circumference's in pixels at zoom level 21
RADIUS = OFFSET / math.pi

def get_pixel(x, y, x_center, y_center, zoom_level):
    """
    x, y - location in degrees
    x_center, y_center - center of the map in degrees (same value as in the google static maps URL)
    zoom_level - same value as in the google static maps URL
    x_ret, y_ret - position of x, y in pixels relative to the center of the bitmap
    """
    x_ret = (l_to_x(x) - l_to_x(x_center)) >> (21 - zoom_level)
    y_ret = (l_to_y(y) - l_to_y(y_center)) >> (21 - zoom_level)
    return x_ret, y_ret

def l_to_x(x):
    return int(round(OFFSET + RADIUS * x * math.pi / 180))

def l_to_y(y):
    return int(round(OFFSET - RADIUS * math.log((1 + math.sin(y * math.pi / 180)) / (1 - math.sin(y * math.pi / 180))) / 2))

print(get_pixel(-22.6264687, 63.9867668, -22.6264687, 63.9867668, 10))
print(get_pixel(-22.6164687, 63.9867668, -22.6264687, 63.9867668, 10))
print(get_pixel(-22.60542555, 63.99183534, -22.6264687, 63.9867668, 10))