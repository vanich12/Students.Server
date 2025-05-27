import { useCallback, useEffect, useRef, useState } from 'react'

export const useDebounceValue = (value, delay = 500) => {
    const [debounceValue, setDebounceValue] = useState(value)

    useEffect(() => {
        const timeout = setTimeout(() => {
            setDebounceValue(value)
        }, delay);

        return () => clearTimeout(timeout);
        }, [value])

    return debounceValue;
}


export const useDebouncedFunction = (func, delay) => {
    // 1. Ссылка на ID таймера. Нам нужно хранить ID, чтобы отменять таймер.
    // так как
    const timeoutIdRef = useRef(null);

    // 2. Ссылка на саму функцию. Чтобы всегда вызывать последнюю версию,
    //    даже если originalFunction изменилась с момента создания debounced обертки.
    const functionRef = useRef(func);

    useEffect(() => {
        functionRef.current = func; // Обновляем, если originalFunction изменилась
    }, [func]);


    // 3. Это та функция, которую мы ВОЗВРАЩАЕМ из хука.
    //    Ее будут вызывать пользователи хука.
    const debouncedVersion = useCallback((...args) => {
        // а) Если уже есть запланированный вызов (таймер), отменяем его.
        //    Это ключевой момент debounce!
        if (timeoutIdRef.current) {
            clearTimeout(timeoutIdRef.current);
        }

        // б) Запускаем новый таймер.
        //    Через 'delay' миллисекунд вызовется функция, указанная внутри setTimeout.
        timeoutIdRef.current = setTimeout(() => {
            // в) Когда таймер сработал, вызываем ОРИГИНАЛЬНУЮ функцию
            //    с ТЕМИ АРГУМЕНТАМИ, которые были переданы в debouncedVersion.
            functionRef.current(...args);
        }, delay);
    }, [delay]); // Зависит только от delay. originalFunction обрабатывается через ref.


    // 4. Очистка таймера при размонтировании компонента.
    //    Если компонент исчезнет, а таймер еще тикает, мы не хотим, чтобы
    //    функция вызвалась "в пустоту" или вызвала ошибку.
    useEffect(() => {
        return () => {
            if (timeoutIdRef.current) {
                clearTimeout(timeoutIdRef.current);
            }
        };
    }, []); // Пустой массив зависимостей = сработает только при монтировании и размонтировании

    return debouncedVersion;
};

