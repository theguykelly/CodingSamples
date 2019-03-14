const debounce = (fn, delay) => {
  var timeout;
  var debounced = function () {
    var args = arguments,
      context = this;

    function later() {
      fn.call(context, args);
    }
    if (timeout) {
      clearTimeout(timeout);
    }
    timeout = setTimeout(later, delay);
  }
  return debounced
}

export {
  debounce
};